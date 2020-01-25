using System;

namespace Infrastracture.Options
{
    public class JwtOptions
    {
        public TimeSpan TokenLifeTime { get; set; }
        public string Secret { get; set; }
    }
}
