using Geex.Common.Abstractions;
using Geex.Common.Identity.Api;
using Volo.Abp.Modularity;

namespace Geex.Common.Identity.Core
{
    [DependsOn(typeof(IdentityApiModule),
        typeof(GeexCoreModule))]
    public class IdentityCoreModule : GeexModule<IdentityCoreModule>
    {

    }
}
