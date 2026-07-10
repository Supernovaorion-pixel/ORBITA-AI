# ORBITA AI — Error Handling

> Ce document définit les principes de gestion des erreurs que le système doit respecter. Aucun mécanisme technique particulier n'est prescrit.

## 1. Principe général

Une erreur n'est jamais silencieuse. Tout échec, anomalie ou situation imprévue doit être détecté, journalisé et communiqué de façon appropriée à son destinataire (utilisateur, administrateur ou système), conformément à l'exigence de confiance qui fonde le produit (cf. [docs/PROJECT_VISION.md](../docs/PROJECT_VISION.md) §4).

## 2. Catégories d'erreurs

- **Erreur de donnée** — une donnée entrante ne respecte pas les règles attendues (cf. `Import Engine`, [DATA_FLOW.md](DATA_FLOW.md) §2). Elle doit être rejetée à l'entrée, jamais intégrée puis corrigée après coup.
- **Erreur de règle métier** — une action demandée viole une règle du Domaine (ex. : rattacher une Facture à un Client d'une autre Organisation). Elle doit être bloquée au niveau du Domaine, quelle que soit la couche d'où provient la demande.
- **Erreur d'accès** — un utilisateur tente une action hors de ses droits (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §5). Elle doit être bloquée avant toute exécution, et journalisée par le module `Audit`.
- **Erreur d'infrastructure** — une dépendance externe (source de données, connecteur) est indisponible ou se comporte de façon inattendue. Elle ne doit jamais se propager telle quelle jusqu'au Domaine ou à l'utilisateur.
- **Erreur inattendue** — toute situation non anticipée par les catégories précédentes. Elle doit être capturée de façon générique, sans jamais interrompre silencieusement le système.

## 3. Journalisation des erreurs

- Toute erreur, quelle que soit sa catégorie, est journalisée avec un niveau de détail suffisant pour permettre un diagnostic ultérieur (cf. [LOGGING_STRATEGY.md](LOGGING_STRATEGY.md)), sans jamais exposer de donnée confidentielle dans un journal accessible à un périmètre plus large que nécessaire.
- La journalisation d'une erreur est systématique et ne dépend jamais de la présence ou non d'un utilisateur pour la constater.

## 4. Messages utilisateur

- Un message d'erreur présenté à l'utilisateur doit être **clair, factuel et actionnable** : il indique ce qui n'a pas pu être réalisé et, lorsque c'est possible, ce que l'utilisateur peut faire.
- Un message d'erreur ne doit jamais exposer de détail technique interne (structure du système, cause technique précise) : ce niveau de détail reste réservé à la journalisation destinée au diagnostic et au support.
- Le ton du message reste conforme à la personnalité de marque définie dans [branding/BRAND_GUIDELINES.md](../branding/BRAND_GUIDELINES.md) : direct, sobre, sans familiarité ni dramatisation.

## 5. Récupération

- Une erreur d'infrastructure ou d'indisponibilité temporaire doit permettre une reprise sans perte de cohérence : une opération interrompue (import, calcul de projection) doit pouvoir être reprise ou relancée sans dupliquer ni corrompre l'état déjà validé.
- Une erreur de donnée ou de règle métier ne doit jamais laisser le système dans un état intermédiaire incohérent : soit l'opération aboutit intégralement, soit elle n'a aucun effet.
- La disponibilité des fonctionnalités non affectées par une erreur doit être préservée : une défaillance localisée à un module ne doit jamais compromettre le fonctionnement des autres (cf. principe d'indépendance des modules, [SYSTEM_ARCHITECTURE.md](SYSTEM_ARCHITECTURE.md) §6).

## 6. Traçabilité

- Chaque erreur doit pouvoir être retracée jusqu'à son origine : l'action, l'utilisateur (le cas échéant) et le module à son origine.
- Cette traçabilité s'appuie sur les mêmes principes que ceux définis pour l'Audit (cf. [LOGGING_STRATEGY.md](LOGGING_STRATEGY.md)) et pour l'intégrité des données (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §2).

## 7. Principe transversal

La gestion des erreurs n'est pas un traitement ajouté après coup à une fonctionnalité : elle fait partie intégrante de sa conception, au même titre que son comportement attendu en situation normale. Une fonctionnalité qui ne définit pas explicitement son comportement en cas d'erreur n'est pas considérée comme terminée.
