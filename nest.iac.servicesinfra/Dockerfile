FROM pulumi/pulumi-dotnet:latest

WORKDIR /work/solution

COPY . /work/solution

COPY ./nest.iac.servicesinfra/pulumi_commands.sh /usr/local/bin/pulumi_commands.sh
RUN sed -i 's/\r$//' /usr/local/bin/pulumi_commands.sh && \
    chmod +x /usr/local/bin/pulumi_commands.sh

WORKDIR /work/solution/nest.iac.servicesinfra
RUN dotnet restore

ENTRYPOINT ["/usr/local/bin/pulumi_commands.sh"]