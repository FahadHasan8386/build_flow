using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Identity.RegisterTenant;

public record RegisterTenantCommand( RegisterTenantRequest Request) : IRequest<RegisterTenantResponse>;

