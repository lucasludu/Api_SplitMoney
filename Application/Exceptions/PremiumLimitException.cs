using System;

namespace Application.Exceptions
{
    public class PremiumLimitException : Exception
    {
        public string FeatureName { get; }
        public int CurrentLimit { get; }

        public PremiumLimitException(string featureName, int currentLimit) 
            : base($"Has alcanzado el límite de {currentLimit} {featureName} para tu plan gratuito. ¡Pásate a Premium para desbloquear acceso ilimitado!")
        {
            FeatureName = featureName;
            CurrentLimit = currentLimit;
        }
    }
}
