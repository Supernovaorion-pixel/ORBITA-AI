# ORBITA AI — Performance Guidelines

> Ce document définit les principes de performance que l'architecture doit garantir. Aucune technologie, aucun mécanisme d'optimisation technique particulier n'est prescrit.

## 1. Exigence de départ

Le système doit rester réactif pour des organisations traitant plusieurs millions de lignes de données, plusieurs années d'historique, un nombre croissant d'utilisateurs simultanés, de grands tableaux de données et des calculs d'analyse ou de projection complexes. La performance est un critère de conception dès l'origine, jamais une correction a posteriori (cf. [docs/TECHNICAL_SPECIFICATION.md](../docs/TECHNICAL_SPECIFICATION.md) §5).

## 2. Principes relatifs au volume de données

- Le Domaine et les moteurs d'analyse doivent être conçus pour raisonner sur des **sous-ensembles pertinents** de données (période, périmètre, territoire) plutôt que sur l'intégralité de l'historique à chaque opération.
- L'agrégation et la pré-consolidation de données fréquemment consultées (cf. module `History`) doivent être anticipées comme un principe de conception, sans imposer de recalcul complet à chaque consultation.
- La croissance du volume de données d'une Organisation ne doit jamais dégrader la performance perçue par une autre Organisation (cf. cloisonnement, [DOMAIN_MODEL.md](DOMAIN_MODEL.md) §4).

## 3. Principes relatifs aux calculs complexes

- Les traitements longs (import massif, projection sur un large périmètre) doivent être conçus comme des opérations asynchrones, découplées de l'interaction immédiate de l'utilisateur, en s'appuyant sur le système d'événements (cf. [EVENT_SYSTEM.md](EVENT_SYSTEM.md)).
- Un calcul complexe ne doit jamais bloquer la consultation d'informations déjà disponibles : la disponibilité de l'information existante prime sur l'attente d'un nouveau calcul.
- Toute opération d'analyse ou de projection doit être conçue pour être interrompue et reprise sans perte de cohérence, plutôt que de devoir être recommencée intégralement en cas d'incident.

## 4. Principes relatifs à la restitution (Dashboard, tableaux)

- La couche Présentation ne doit jamais recevoir plus de données que ce qui est nécessaire à l'affichage courant : l'agrégation et le filtrage sont réalisés en amont, au niveau de l'Application ou du Domaine.
- Un grand tableau de données doit être conçu pour être consulté par fragments (pagination, chargement progressif) plutôt que dans son intégralité en une seule fois.
- Le temps de réponse perçu par l'utilisateur est un indicateur de qualité au même titre que l'exactitude du résultat (cf. [docs/TECHNICAL_SPECIFICATION.md](../docs/TECHNICAL_SPECIFICATION.md) §5).

## 5. Principes relatifs à la montée en charge (utilisateurs multiples)

- L'architecture modulaire (cf. [SYSTEM_ARCHITECTURE.md](SYSTEM_ARCHITECTURE.md)) permet à chaque module de monter en charge indépendamment des autres, sans qu'une forte sollicitation d'un module (par exemple `Reporting Engine` lors d'une période de clôture) ne dégrade la disponibilité d'un autre (par exemple `Dashboard`).
- Le cloisonnement strict par Organisation garantit que la charge générée par une Organisation ne peut jamais affecter la performance perçue par une autre.

## 6. Principes relatifs à l'historique de plusieurs années

- La conservation de plusieurs années de données ne doit jamais imposer que chaque consultation courante parcoure l'intégralité de cet historique : la donnée récente et fréquemment consultée doit rester la plus rapide d'accès, sans que la donnée ancienne ne soit pour autant moins fiable ou moins traçable (cf. [DATA_FLOW.md](DATA_FLOW.md) §7).

## 7. Principe d'évaluation continue

Toute nouvelle fonctionnalité doit être évaluée, dès sa conception, sur son comportement face à un volume de données représentatif d'une organisation Enterprise à pleine échelle (plusieurs millions de lignes, plusieurs années d'historique), et non uniquement sur un jeu de données réduit. Une fonctionnalité qui ne résiste pas à cette échelle doit être repensée avant d'être considérée comme terminée.

## 8. Cohérence avec la modularité

La performance ne doit jamais être obtenue au prix d'un couplage direct entre modules (raccourci technique reliant deux modules pour "gagner du temps") : tout gain de performance doit rester compatible avec les règles de dépendance définies dans [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md). Un arbitrage entre performance et modularité doit être documenté explicitement (cf. [ARCHITECTURE_DECISIONS.md](ARCHITECTURE_DECISIONS.md)).
