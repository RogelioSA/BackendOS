using System.Net;
using Aws = Pulumi.Aws;
using Awsx = Pulumi.Awsx;

namespace nest.iac.servicesinfra.Resources
{
    public class FargateCreator
    {
        private readonly string taskName;
        private readonly string serviceName;
        private readonly string containerName;
        private readonly string lbName;
        private readonly string listenerName;
        private readonly string targetGroupName;
        private readonly string targetScallingName;
        private readonly string basePath;
        private readonly string ruleName;
        private readonly int priority;
        private readonly Aws.Iam.Role executionRole;
        private readonly Aws.Iam.Role taskRole;
        private Awsx.Ecr.Image image;
        private Aws.CloudWatch.LogGroup logGroup;
        private int port;
        private Aws.LB.LoadBalancer loadBalancer = null!;
        private Aws.AppAutoScaling.Target target = null!;
        private Aws.Ecs.TaskDefinition task = null!;
        private Aws.LB.TargetGroup targetGroup = null!;
        private Aws.LB.Listener listener = null!;
        private Aws.LB.ListenerRule rule = null!;
        private Aws.Ecs.Service service = null!;
        public FargateCreator(string prefix, string shortPrefix, Aws.Iam.Role executionRole, Aws.Iam.Role taskRole, Awsx.Ecr.Image image, Aws.CloudWatch.LogGroup logGroup, Aws.LB.Listener listener, int port, string basePath, string ruleName, int priority)
        {
            this.taskName = $"{prefix}-task";
            this.serviceName = $"{prefix}-svc";
            this.containerName = $"{prefix}-container";
            this.targetGroupName = $"{shortPrefix}-tg";
            this.targetScallingName = $"{prefix}-tscalling";
            this.executionRole = executionRole;
            this.taskRole = taskRole;
            this.image = image;
            this.logGroup = logGroup;
            this.port = port;
            this.basePath = basePath;
            this.ruleName = ruleName;
            this.listener = listener;
            this.priority = priority;
        }
        public FargateCreator(string prefix)
        {
            this.lbName = $"{prefix}-lb";
            this.listenerName = $"{prefix}-listener";
        }
        public Aws.Ecs.Service Build()
        {
            this.task = CreateTask();
            this.targetGroup = CreateTargetGroup();
            this.rule = CreateRule();
            this.service = this.CreateService();
            return this.service;
        }
        public (Aws.LB.Listener, Aws.LB.LoadBalancer) BuildLb()
        {
            this.loadBalancer = CreateLoadBalancer();
            this.listener = CreateListener();
            return (this.listener, this.loadBalancer);
        }
        public Aws.Ecs.TaskDefinition CreateTask()
        {
            return new Aws.Ecs.TaskDefinition(this.taskName, new Aws.Ecs.TaskDefinitionArgs
            {
                Family = this.taskName,
                Cpu = "256",
                Memory = "512",
                NetworkMode = "awsvpc",
                RequiresCompatibilities = { "FARGATE" },
                ExecutionRoleArn = executionRole.Arn,
                TaskRoleArn = taskRole.Arn,
                ContainerDefinitions = Pulumi.Output.All(this.image.ImageUri, this.logGroup.Name).Apply(variables => {
                    return $@"[ {{
                        ""name"": ""{this.containerName}"",
                        ""image"": ""{variables[0]}"",
                        ""essential"": true,
                        ""portMappings"": [ {{
                            ""containerPort"": {port},
                            ""hostPort"": {port}
                        }} ],
                        ""environment"": [ {{
                            ""name"": ""ASPNETCORE_ENVIRONMENT"",
                            ""value"": ""Development""
                        }},{{
                            ""name"": ""ENGINE"",
                            ""value"": ""Npgsql""
                        }},{{
                            ""name"": ""Connections__Npgsql"",
                            ""value"": ""Host=nest-generalinfra-instance.cibyifu5bsuf.us-east-1.rds.amazonaws.com;Port=5432;Database=nest;Username=lucia;Password=123Lucia01*;Application Name=Nest;""
                        }},{{
                            ""name"": ""BASE_URL"",
                            ""value"": ""{this.basePath}""
                        }} ],
                        ""logConfiguration"": {{
                            ""logDriver"": ""awslogs"",
                            ""options"": {{
                                ""awslogs-group"": ""{variables[1]}"",
                                ""awslogs-region"": ""{Aws.Config.Region}"",
                                ""awslogs-stream-prefix"": ""ecs""
                            }}
                        }}
                    }} ]";
                }

                )

            });
        }

        private Aws.LB.LoadBalancer CreateLoadBalancer()
        {
            return new Aws.LB.LoadBalancer(this.lbName, new Aws.LB.LoadBalancerArgs
            {
                Name = this.lbName,
                Internal = true,
                LoadBalancerType = "application",
                SecurityGroups = ConfigVariables.AwsSecurityGroups,
                Subnets = ConfigVariables.AwsSubnets,
                EnableDeletionProtection = false
            });
        }

        private Aws.AppAutoScaling.Target CreateTargetScalling()
        {
            return new Aws.AppAutoScaling.Target(targetScallingName, new Aws.AppAutoScaling.TargetArgs {
                ServiceNamespace = "ecs",
                ScalableDimension = "ecs:service:DesiredCount",
                ResourceId = $"service/{ConfigVariables.AwsClusterName}/{serviceName}",
                MinCapacity = 1,
                MaxCapacity = 1
            });
        }

        private Aws.LB.Listener CreateListener()
        {
            return new Aws.LB.Listener(this.listenerName, new Aws.LB.ListenerArgs {
                LoadBalancerArn = loadBalancer.Arn,
                Port = 80,
                Protocol = "HTTP",
                DefaultActions = new Pulumi.InputList<Aws.LB.Inputs.ListenerDefaultActionArgs> {
                    new Aws.LB.Inputs.ListenerDefaultActionArgs {
                        Type = "fixed-response",
                        FixedResponse = new Aws.LB.Inputs.ListenerDefaultActionFixedResponseArgs {
                            ContentType = "text/plain",
                            MessageBody = "Not Found",
                            StatusCode = "404"
                        }
                    }
                }
            });
        }

        private Aws.LB.TargetGroup CreateTargetGroup()
        {
            return new Aws.LB.TargetGroup(this.targetGroupName, new Aws.LB.TargetGroupArgs {
                Name = this.targetGroupName,
                Port = this.port,
                Protocol = "HTTP",
                VpcId = ConfigVariables.AwsVpcId,
                TargetType = "ip",
                HealthCheck = new Aws.LB.Inputs.TargetGroupHealthCheckArgs {
                    Path = "/health/live",
                    Protocol = "HTTP",
                    Matcher = "200",
                    Interval = 30,
                    Timeout = 5,
                    HealthyThreshold = 3,
                    UnhealthyThreshold = 2
                }
            });
        }

        private Aws.Ecs.Service CreateService()
        {
            return new Aws.Ecs.Service(this.serviceName, new Aws.Ecs.ServiceArgs {
                Name = this.serviceName,
                LaunchType = "FARGATE",
                TaskDefinition = this.task.Arn,
                Cluster = ConfigVariables.AwsClusterArn,
                DesiredCount = 1,
                ForceNewDeployment = true,
                HealthCheckGracePeriodSeconds = 480,
                NetworkConfiguration = new Aws.Ecs.Inputs.ServiceNetworkConfigurationArgs
                {
                    SecurityGroups = ConfigVariables.AwsSecurityGroups,
                    Subnets = ConfigVariables.AwsSubnets,
                    AssignPublicIp = true
                },
                LoadBalancers = new Pulumi.InputList<Aws.Ecs.Inputs.ServiceLoadBalancerArgs> {
                    new Aws.Ecs.Inputs.ServiceLoadBalancerArgs {
                        TargetGroupArn = targetGroup.Arn,
                        ContainerName = this.containerName,
                        ContainerPort = this.port
                    }
                },
                PropagateTags = "SERVICE"
            });
        }

        private Aws.LB.ListenerRule CreateRule()
        {
            return new Aws.LB.ListenerRule(this.ruleName, new()
            {
                ListenerArn = this.listener.Arn,
                Priority = this.priority,
                Actions = {
                new Aws.LB.Inputs.ListenerRuleActionArgs {
                    Type = "forward",
                    TargetGroupArn = this.targetGroup.Arn
                }
            },
                Conditions = {
                new Aws.LB.Inputs.ListenerRuleConditionArgs {
                    PathPattern = new Aws.LB.Inputs.ListenerRuleConditionPathPatternArgs {
                        Values = { $"{this.basePath}/*" }
                    }
                }
            }
            });
        }

    }
}
