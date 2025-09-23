#!/usr/bin/env bash
set -euo pipefail

# Pulumi local backend
pulumi login

# Selecciona el stack "dev"
pulumi stack select ${PULUMI_STACK} --non-interactive --create

# Ejecuta la actualización
pulumi refresh --yes
pulumi up --yes
#pulumi destroy --yes --stack "${PULUMI_STACK}"
#pulumi stack rm --yes "${PULUMI_STACK}"