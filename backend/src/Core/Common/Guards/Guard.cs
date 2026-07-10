namespace OrbitaAI.Core.Common.Guards;

/// <summary>
/// Point d'entrée des clauses de garde officielles du projet. Une clause de garde valide une
/// précondition à l'entrée d'une méthode et lève immédiatement une exception explicite en cas
/// de violation, plutôt que de laisser une donnée invalide se propager silencieusement
/// (architecture/CODING_PRINCIPLES.md §7, architecture/ERROR_HANDLING.md).
/// </summary>
public static class Guard
{
    /// <summary>Regroupe l'ensemble des vérifications disponibles (ex. <c>Guard.Against.Null(value, nameof(value))</c>).</summary>
    public static class Against
    {
        /// <summary>Lève <see cref="ArgumentNullException"/> si <paramref name="value"/> est <see langword="null"/>.</summary>
        public static T Null<T>(T? value, string parameterName) where T : class
            => value ?? throw new ArgumentNullException(parameterName);

        /// <summary>Lève <see cref="ArgumentNullException"/> si <paramref name="value"/> est <see langword="null"/> (types valeur nullables).</summary>
        public static T NullStruct<T>(T? value, string parameterName) where T : struct
            => value ?? throw new ArgumentNullException(parameterName);

        /// <summary>Lève <see cref="ArgumentException"/> si <paramref name="value"/> est <see langword="null"/> ou vide.</summary>
        public static string NullOrEmpty(string? value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("La valeur ne peut être ni nulle ni vide.", parameterName);
            }

            return value;
        }

        /// <summary>Lève <see cref="ArgumentException"/> si <paramref name="value"/> est <see langword="null"/>, vide ou composé uniquement d'espaces.</summary>
        public static string NullOrWhiteSpace(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("La valeur ne peut être ni nulle, ni vide, ni composée uniquement d'espaces.", parameterName);
            }

            return value;
        }

        /// <summary>Lève <see cref="ArgumentException"/> si <paramref name="value"/> vaut <see cref="Guid.Empty"/>.</summary>
        public static Guid Default(Guid value, string parameterName)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("L'identifiant ne peut pas être vide.", parameterName);
            }

            return value;
        }

        /// <summary>Lève <see cref="ArgumentOutOfRangeException"/> si <paramref name="value"/> est strictement négatif.</summary>
        public static int Negative(int value, string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, "La valeur ne peut pas être négative.");
            }

            return value;
        }

        /// <summary>Lève <see cref="ArgumentOutOfRangeException"/> si <paramref name="value"/> est négatif ou nul.</summary>
        public static int NegativeOrZero(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, "La valeur doit être strictement positive.");
            }

            return value;
        }

        /// <summary>Lève <see cref="ArgumentOutOfRangeException"/> si <paramref name="value"/> est en dehors de l'intervalle [<paramref name="min"/>, <paramref name="max"/>].</summary>
        public static int OutOfRange(int value, int min, int max, string parameterName)
        {
            if (value < min || value > max)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, $"La valeur doit être comprise entre {min} et {max}.");
            }

            return value;
        }

        /// <summary>Lève <see cref="ArgumentException"/> si <paramref name="value"/> ne correspond à aucun membre défini de l'énumération <typeparamref name="TEnum"/>.</summary>
        public static TEnum OutOfEnum<TEnum>(TEnum value, string parameterName) where TEnum : struct, Enum
        {
            if (!Enum.IsDefined(value))
            {
                throw new ArgumentException($"La valeur '{value}' n'est pas définie dans l'énumération {typeof(TEnum).Name}.", parameterName);
            }

            return value;
        }
    }
}
