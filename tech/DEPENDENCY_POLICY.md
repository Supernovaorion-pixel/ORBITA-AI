# ORBITA AI — Dependency Policy

> Ce document définit la politique officielle de gestion des dépendances tierces du projet, backend et frontend.

## 1. Principe général

Une dépendance tierce est un engagement à long terme, pas une commodité ponctuelle : chaque dépendance introduite doit pouvoir être maintenue, mise à jour et, si nécessaire, remplacée sans remettre en cause la stabilité du système sur dix ans (cf. [architecture/TECHNICAL_SPECIFICATION.md](../architecture/TECHNICAL_SPECIFICATION.md) §3).

## 2. Critères d'ajout d'une dépendance

Une nouvelle dépendance n'est acceptée que si elle satisfait l'ensemble des critères suivants :

1. **Nécessité réelle** — la capacité recherchée ne peut être obtenue simplement par le socle déjà retenu (cf. [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md)) sans effort disproportionné (principe YAGNI, [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §4).
2. **Maturité** — la dépendance dispose d'un historique de publication d'au moins plusieurs années et d'un rythme de maintenance régulier et documenté.
3. **Communauté et adoption** — la dépendance est utilisée par un nombre significatif de projets tiers indépendants, réduisant le risque d'abandon soudain.
4. **Licence compatible** — la licence de la dépendance est compatible avec une distribution commerciale du logiciel, y compris en mode On-Premise, sans obligation de rétrocession du code propriétaire d'ORBITA AI.
5. **Absence de dépendance transitive excessive** — la dépendance n'entraîne pas elle-même un nombre disproportionné de dépendances transitives peu maîtrisées.
6. **Alignement avec le socle retenu** — la dépendance s'intègre nativement à l'écosystème .NET ou à l'écosystème TypeScript/React déjà retenu (cf. [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md)), sans introduire un langage ou runtime supplémentaire.

Toute dépendance ne satisfaisant pas l'un de ces critères doit faire l'objet d'une décision documentée explicite (cf. [architecture/ARCHITECTURE_DECISIONS.md](../architecture/ARCHITECTURE_DECISIONS.md)) avant d'être acceptée à titre d'exception.

## 3. Bibliothèques autorisées (catégories)

Sont autorisées, sous réserve de satisfaire les critères ci-dessus, les catégories de bibliothèques suivantes :
- bibliothèques de test (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md)),
- bibliothèques de composants d'interface strictement complémentaires au [ux/COMPONENT_LIBRARY.md](../ux/COMPONENT_LIBRARY.md) (ex. rendu de graphiques conforme à [ux/DATA_VISUALIZATION.md](../ux/DATA_VISUALIZATION.md)),
- bibliothèques de génération de documents (PDF, Excel, PowerPoint, cf. [features/REPORTING_ENGINE.md](../features/REPORTING_ENGINE.md)),
- bibliothèques d'accès aux données et de migration de schéma pour PostgreSQL (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md)),
- bibliothèques de journalisation conformes à [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md),
- clients officiels ou largement adoptés pour l'intégration de fournisseurs d'intelligence artificielle (cf. [AI_INTEGRATION.md](AI_INTEGRATION.md)), toujours utilisés derrière l'abstraction définie dans ce document.

## 4. Bibliothèques interdites

Sont explicitement interdites :
- toute bibliothèque dont la maintenance est arrêtée ou dont la dernière publication remonte à plus de deux ans sans explication (fin de vie déclarée, stabilité totale documentée),
- toute bibliothèque dupliquant une capacité déjà couverte par une dépendance existante du projet (principe DRY appliqué aux dépendances),
- toute bibliothèque à licence restrictive incompatible avec la distribution commerciale ou imposant la publication du code source propriétaire,
- tout framework d'interface ou d'accès aux données concurrent de ceux déjà retenus dans [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md), sauf remplacement complet et documenté de ce dernier,
- toute bibliothèque nécessitant une dépendance à un service en ligne propriétaire non substituable pour faire fonctionner une fonctionnalité de base du produit, ce qui compromettrait le fonctionnement On-Premise.

## 5. Politique de mise à jour

- Les dépendances sont revues à un rythme régulier et documenté, avec une priorité absolue donnée aux mises à jour corrigeant une vulnérabilité de sécurité connue (cf. [SECURITY_REQUIREMENTS.md](SECURITY_REQUIREMENTS.md)).
- Une mise à jour majeure d'une dépendance (rupture de compatibilité potentielle) est traitée comme un changement à part entière, testée selon [TESTING_STRATEGY.md](TESTING_STRATEGY.md) avant toute fusion, jamais appliquée en urgence sans vérification hors contexte de vulnérabilité critique.
- Une dépendance qui cesse de satisfaire les critères du §2 (abandon constaté, faille non corrigée) doit être remplacée, jamais conservée par défaut.

## 6. Traçabilité

- L'ensemble des dépendances utilisées, avec leur version exacte, est déclaré de façon centralisée et versionnée (cf. [DEVELOPMENT_ENVIRONMENT.md](DEVELOPMENT_ENVIRONMENT.md) §2), permettant un audit complet à tout moment, exigence particulièrement importante pour les Organisations clientes Enterprise soumises à des obligations de conformité (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md)).
