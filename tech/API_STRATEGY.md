# ORBITA AI — API Strategy

> Ce document définit la stratégie d'interface de programmation d'ORBITA AI, interne et future publique, préfigurée par les principes d'extensibilité définis dans [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §10.

## 1. Principe directeur

L'ensemble des échanges entre la couche Présentation et l'Application, entre modules exposant une interface, et à terme avec des systèmes tiers, repose sur un style d'interface unique, uniforme et documenté : une API organisée autour des ressources du Domaine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)), échangeant des messages au format JSON (cf. [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md) §6), transportée sur un protocole web standard et largement supporté.

## 2. API interne

- L'API interne est l'interface par laquelle la couche Présentation (cf. [architecture/APPLICATION_LAYERS.md](../architecture/APPLICATION_LAYERS.md)) sollicite la couche Application de chaque module.
- Elle expose exclusivement des cas d'usage définis dans [features/](../features/), jamais un accès direct aux structures internes du Domaine.
- Elle n'est pas destinée à un usage par un système tiers : sa stabilité est garantie au sein d'une même version du produit, mais elle peut évoluer plus librement qu'une API publique dès lors que la Présentation est mise à jour en cohérence (même base de code, cf. [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) §4).

## 3. API publique (future)

- Bien qu'aucune API publique ne soit développée à ce stade (cf. [features/FUTURE_FEATURES.md](../features/FUTURE_FEATURES.md) §4), l'API interne est conçue dès l'origine selon les mêmes exigences de rigueur (versionnement, documentation, stabilité des contrats) qu'une future API publique, afin que son ouverture ultérieure ne nécessite qu'une exposition contrôlée de capacités déjà existantes, jamais une réécriture.
- Une future API publique permettra l'accès programmatique aux données et fonctionnalités d'une Organisation cliente par des systèmes tiers qu'elle autorise explicitement (cf. [features/CONNECTORS.md](../features/CONNECTORS.md) §4), selon les mêmes règles d'autorisation et de cloisonnement que l'accès via l'interface utilisateur standard (cf. [SECURITY_REQUIREMENTS.md](SECURITY_REQUIREMENTS.md) §2).

## 4. Versionnement

- Chaque version majeure de l'API (interne ou, à terme, publique) est identifiée explicitement (ex. segment de version dans le chemin d'accès), conformément au Semantic Versioning défini dans [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md).
- Une évolution mineure ou corrective de l'API n'affecte jamais les intégrations existantes (ajout de champ optionnel, jamais de suppression ou de changement de sens d'un champ existant sans changement de version majeure).
- Plusieurs versions majeures de l'API peuvent coexister temporairement lors d'une transition, selon la même logique de compatibilité ascendante définie dans [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §8.

## 5. Compatibilité

- Toute évolution de l'API respecte le principe de compatibilité ascendante : un système déjà intégré à une version donnée de l'API continue de fonctionner sans modification tant que cette version reste supportée (cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §7 appliqué aux interfaces de module).
- Une dépréciation d'une capacité de l'API est toujours annoncée à l'avance, avec une période de transition raisonnable avant son retrait effectif, jamais un retrait immédiat et silencieux.

## 6. Sécurité de l'API

- Toute requête à l'API est authentifiée et autorisée selon les mêmes principes que l'accès via l'interface utilisateur (cf. [SECURITY_REQUIREMENTS.md](SECURITY_REQUIREMENTS.md) §1-2), sans exception ni raccourci de sécurité propre à l'API.
- Toute requête à l'API est journalisée conformément à [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3.

## 7. Documentation

- L'API interne comme la future API publique disposent d'une documentation technique générée depuis les mêmes annotations de code que celles définies dans [DEVELOPMENT_ENVIRONMENT.md](DEVELOPMENT_ENVIRONMENT.md) §6, garantissant que la documentation d'API ne diverge jamais de son implémentation réelle.

## 8. Cohérence avec l'architecture modulaire

L'API respecte strictement les règles de dépendance entre modules définies dans [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) : elle ne crée jamais de raccourci d'accès direct à un module qui contournerait ces règles.
