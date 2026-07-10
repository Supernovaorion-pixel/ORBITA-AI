# ORBITA AI — Data Flow

> Ce document décrit le cheminement logique de la donnée à travers le système, indépendamment de toute technologie de transport, de stockage ou de traitement.

## 1. Vue d'ensemble

Le flux de données d'ORBITA AI suit six étapes logiques successives, formant un cycle continu :

```
Entrée → Traitement → Analyse → Résultats → Export
                ↓                              ↓
            Historisation ←────────────────────┘
```

Chaque étape correspond à une responsabilité distincte, portée par un ou plusieurs modules définis dans [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md).

## 2. Entrée

- La donnée entre dans le système par le module **Import Engine**, en provenance de sources externes à l'organisation cliente (historique de ventes, pipeline, référentiels, données client).
- À ce stade, la donnée est contrôlée : format attendu, cohérence minimale, absence de doublon avec une donnée déjà intégrée.
- Aucune donnée n'entre dans les étapes suivantes sans avoir satisfait ce contrôle d'entrée.

## 3. Traitement

- La donnée entrante est structurée et rattachée aux entités du Domaine (cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md)) : Organisation, Commercial, Client, Produit, etc.
- Le traitement consolide la donnée dans la source de référence de l'organisation, sans en altérer le sens d'origine.
- Cette étape garantit que toute donnée, quelle que soit sa source, est représentée de façon homogène pour les étapes suivantes.

## 4. Analyse

- Les données consolidées sont exploitées par les moteurs d'analyse (**Analytics Engine**, **Forecast Engine**) pour produire des comparaisons, tendances, décompositions et projections.
- Cette étape ne modifie jamais la donnée source : elle produit des résultats dérivés, toujours traçables jusqu'à leur origine (cf. principe d'intégrité, [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md)).

## 5. Résultats

- Les résultats d'analyse sont mis à disposition sous forme exploitable par : le **Dashboard** (restitution visuelle), **ORION** (interprétation en langage naturel), le **Reporting Engine** (formalisation en rapport) et le **Notification Center** (alertes).
- Un même résultat d'analyse peut alimenter simultanément plusieurs de ces destinations, sans duplication de la logique de calcul.

## 6. Export

- Les résultats et rapports peuvent quitter le système via l'**Export Engine**, vers un usage externe à la plateforme.
- L'export reflète fidèlement l'état de la donnée et de l'analyse au moment où il est produit.

## 7. Historisation

- À chaque étape significative (entrée, traitement, résultat), le module **History** conserve un état daté, permettant de reconstituer la trajectoire complète de la donnée et de la performance dans le temps.
- L'historisation est continue et transversale : elle n'est pas une étape finale isolée, mais accompagne l'ensemble du flux.
- Le module **Audit** enregistre en parallèle toute action ayant affecté ce flux (qui, quoi, quand), à des fins de traçabilité (cf. [LOGGING_STRATEGY.md](LOGGING_STRATEGY.md)).

## 8. Principe de circulation

- Le flux de données circule toujours dans le même sens logique (Entrée → Traitement → Analyse → Résultats → Export), sans retour en arrière qui modifierait une étape antérieure.
- Toute correction d'une donnée déjà intégrée constitue une nouvelle entrée tracée, jamais une modification silencieuse d'une donnée passée.
- Ce principe garantit la cohérence du système avec l'exigence d'intégrité et de traçabilité définie dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md).
