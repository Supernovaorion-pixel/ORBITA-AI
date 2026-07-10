// OrbitaAI.Host — point d'assemblage exécutable de la plateforme (tech/PROJECT_STRUCTURE.md §2).
//
// Squelette architectural uniquement : ce point d'entrée ne démarre aucun service métier,
// n'expose aucune API et n'implémente aucun traitement. Il atteste uniquement que le Core
// et l'ensemble des 16 modules natifs se référencent et se résolvent correctement.
//
// Le choix du modèle d'hébergement définitif (ex. hôte web ASP.NET Core pour exposer
// l'API interne/REST prévue par tech/API_STRATEGY.md) est un point restant, à trancher
// et documenter au démarrage de la Phase 2 — Core (planning/DEVELOPMENT_PHASES.md).

Console.WriteLine("ORBITA AI — Host (squelette architectural, aucune logique métier)");
