using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using Aws = Pulumi.Aws;

namespace nest.iac.servicesinfra.Resources
{
	public class RoleCreator
	{
		private readonly string roleExecutionName;
		private readonly string roleTaskName;
        private readonly string lambdaRoleName;
        private readonly string prefix;
		private readonly string project;

        public RoleCreator(string roleExecutionName, string roleTaskName, string prefix, string project)
		{
			this.roleExecutionName = roleExecutionName;
			this.roleTaskName = roleTaskName;
			this.prefix = prefix;
			this.project = project;
        }
        public RoleCreator(string lambdaRoleName, string prefix, string project)
        {
            this.lambdaRoleName = lambdaRoleName;
            this.prefix = prefix;
            this.project = project;
        }

        public (Aws.Iam.Role, Aws.Iam.Role) Build()
        {
            Aws.Iam.Role executionRole = this.createRoleExecution(roleExecutionName, prefix, project);
            Aws.Iam.Role taskRole = this.createRoleTask(roleTaskName, prefix, project);
			return (executionRole, taskRole);
        }

        public Aws.Iam.Role BuildLambda()
        {
            Aws.Iam.Role lambdaRole = this.createLambdaRole(this.lambdaRoleName, prefix, project);
            return lambdaRole;
        }

        private string getLoggingPolicy()
		{
			return JsonSerializer.Serialize(new Dictionary<string, object?>
			{
				{ "Version", "2012-10-17" },
				{ "Statement", new [] {
					new Dictionary<string, object?>
					{
						{ "Effect", "Allow" },
						{ "Action", new [] { "logs:CreateLogGroup", "logs:CreateLogStream", "logs:PutLogEvents" } },
						{ "Resource", new [] { "arn:aws:logs:*:*:*" } }
					},
				}
			} });
		}
		private string getNetworkPolicy()
		{
            return JsonSerializer.Serialize(new Dictionary<string, object?>
            {
                { "Version", "2012-10-17" },
                { "Statement", new [] {
                    new Dictionary<string, object?>
                    {
                        { "Effect", "Allow" },
                        { "Action", new [] {
                            "ec2:DescribeNetworkInterfaces",
                            "ec2:CreateNetworkInterface",
                            "ec2:DeleteNetworkInterface",
                            "ec2:DescribeInstances",
                            "ec2:AttachNetworkInterface"
                        } },
                        { "Resource", new [] { "*" } }
                    },
                }
            } });
		}
		private string getSecretPolicy()
		{
            return JsonSerializer.Serialize(new Dictionary<string, object?>
            {
                { "Version", "2012-10-17" },
                { "Statement", new [] {
                    new Dictionary<string, object?>
                    {
                        { "Effect", "Allow" },
                        { "Action", new [] {
                            "secretsmanager:*"
                        } },
                        { "Resource", new [] { "*" } }
                    },
                }
            } });
		}
        private string getPrincipalPolicy()
        {
            return JsonSerializer.Serialize(new Dictionary<string, object?>
            {
                ["Version"] = "2012-10-17",
                ["Statement"] = new[]
                {
                    new Dictionary<string, object?>
                    {
                        ["Effect"] = "Allow",
                        ["Principal"] = new Dictionary<string, object?>
                        {
                            ["Service"] = "ecs-tasks.amazonaws.com"
                        },
                        ["Action"] = "sts:AssumeRole"
                    }
                }
            });
        }
        private string getPrincipalLambda()
        {
            return JsonSerializer.Serialize(new Dictionary<string, object?>
            {
                ["Version"] = "2012-10-17",
                ["Statement"] = new[]
                {
                    new Dictionary<string, object?>
                    {
                        ["Effect"] = "Allow",
                        ["Principal"] = new Dictionary<string, object?>
                        {
                            ["Service"] = "lambda.amazonaws.com"
                        },
                        ["Action"] = "sts:AssumeRole"
                    }
                }
            });
        }
        private void addPoliciesToRoleTask(Aws.Iam.Role role, string prefix, string project)
		{
			string policyForLogging = this.getLoggingPolicy();
			string policyForNetwork = this.getNetworkPolicy();
			string policyForSecret = this.getSecretPolicy();
			this.attachPolicyToRole(role, $"{prefix}-ta-logging", policyForLogging, $"Logging policy for the {project}");
			this.attachPolicyToRole(role, $"{prefix}-ta-network", policyForNetwork, $"Network policy for the {project}");
			this.attachPolicyToRole(role, $"{prefix}-ta-secret", policyForSecret, $"Logging policy for the {project}");
		}
		private void attachPolicyToRole(Aws.Iam.Role role, string policyKey, string policy, string description)
		{
			string policyName = $"{policyKey}-policy";
			Aws.Iam.Policy iamPolicy = new Aws.Iam.Policy(policyName, new Aws.Iam.PolicyArgs
			{
				Name = policyName,
				Path = "/",
				Description = description,
				PolicyDocument = policy
			});

			new Aws.Iam.RolePolicyAttachment($"{policyName}-attachment", new Aws.Iam.RolePolicyAttachmentArgs
			{
				Role = role.Name,
				PolicyArn = iamPolicy.Arn,
			});
		}
		private void attachPolicyManagedToRole(Aws.Iam.Role role, string policyName, string policyArn)
		{
			new Aws.Iam.RolePolicyAttachment($"{policyName}-attachment", new Aws.Iam.RolePolicyAttachmentArgs
			{
				Role = role.Name,
				PolicyArn = policyArn
			});
		}
		private void addPoliciesToRoleExecution(Aws.Iam.Role role, string prefix, string project)
		{
			string policyForLogging = this.getLoggingPolicy();
			string policyForSecret = this.getSecretPolicy();
            this.attachPolicyToRole(role, $"{prefix}-et-secret", policyForSecret, $"Logging policy for the {project}");
			this.attachPolicyToRole(role, $"{prefix}-et-logging", policyForLogging, $"Secret policy for the {project}");
			this.attachPolicyManagedToRole(role, $"{prefix}-et-AWSXray", "arn:aws:iam::aws:policy/AWSXrayWriteOnlyAccess");
			this.attachPolicyManagedToRole(role, $"{prefix}-et-ASP", $"arn:aws:iam::aws:policy/CloudWatchAgentServerPolicy");
			this.attachPolicyManagedToRole(role, $"{prefix}-et-ETSK", $"arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy");
		}
        private void addPoliciesToRoleLambda(Aws.Iam.Role role, string prefix, string project)
        {
            string policyForLogging = this.getLoggingPolicy();
            string policyForNetwork = this.getNetworkPolicy();
            string policyForSecret = this.getSecretPolicy();
            this.attachPolicyToRole(role, $"{prefix}-lambda-secret", policyForSecret, $"Logging policy for the {project}");
            this.attachPolicyToRole(role, $"{prefix}-lambda-network", policyForNetwork, $"Network policy for the {project}");
            this.attachPolicyToRole(role, $"{prefix}-lambda-logging", policyForLogging, $"Secret policy for the {project}");
            this.attachPolicyManagedToRole(role, $"{prefix}-lambda-AWSXray", Aws.Iam.ManagedPolicy.AWSLambdaBasicExecutionRole.ToString());
        }
        public Aws.Iam.Role createRoleTask(string roleName, string prefix, string project)
        {
            Aws.Iam.Role role = new Aws.Iam.Role(roleName, new Aws.Iam.RoleArgs
            {
                Name = roleName,
                AssumeRolePolicy = this.getPrincipalPolicy(),
            });
            this.addPoliciesToRoleTask(role, prefix, project);
            return role;
        }
        public Aws.Iam.Role createRoleExecution(string roleName, string prefix, string project)
        {
            Aws.Iam.Role role = new Aws.Iam.Role(roleName, new Aws.Iam.RoleArgs
            {
                Name = roleName,
                AssumeRolePolicy = this.getPrincipalPolicy(),
            });
            this.addPoliciesToRoleExecution(role, prefix, project);
            return role;
        }
        public Aws.Iam.Role createLambdaRole(string roleName, string prefix, string project)
        {
            Aws.Iam.Role role = new Aws.Iam.Role(roleName, new Aws.Iam.RoleArgs
            {
                Name = roleName,
                AssumeRolePolicy = this.getPrincipalLambda()
            });
            this.addPoliciesToRoleLambda(role, prefix, project);
            return role;
        }
    }
}
