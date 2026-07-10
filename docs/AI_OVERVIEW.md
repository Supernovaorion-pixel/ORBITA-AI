# ORBITA AI — AI Overview (ORION)

> Ce document décrit exclusivement le rôle d'ORION au sein d'ORBITA AI : ses responsabilités, ses capacités et ses limites. Aucun modèle, technologie ou méthode d'intelligence artificielle n'est mentionné ou présupposé dans ce document.

## 1. Qui est ORION

ORION est l'assistant intelligent intégré à ORBITA AI. Il constitue le point de contact entre l'utilisateur et l'ensemble des données et analyses de la plateforme (cf. module **ORION**, [FUNCTIONAL_SPECIFICATION.md](FUNCTIONAL_SPECIFICATION.md)). ORION n'est pas un module parmi d'autres : il est conçu comme une couche transversale d'assistance, présente partout où une décision peut être éclairée.

## 2. Responsabilités

ORION a pour responsabilités de :

- **Interpréter** la donnée disponible sur la plateforme pour répondre aux questions posées par l'utilisateur en langage naturel.
- **Synthétiser** l'information dispersée en une réponse claire et directement exploitable.
- **Signaler** de façon proactive les signaux significatifs (écarts, tendances, anomalies) que l'utilisateur n'aurait pas explicitement recherchés.
- **Recommander** des pistes d'action ou d'attention fondées sur l'analyse des données disponibles, sans jamais se substituer à la décision finale de l'utilisateur.

## 3. Capacités

- Répondre à des questions portant sur la performance commerciale de l'organisation, à tous les niveaux (global, régional, individuel), dans les limites du périmètre de données et de droits d'accès de l'utilisateur qui interroge.
- Mettre en relation plusieurs sources de données internes à la plateforme pour construire une réponse cohérente.
- Identifier des écarts, tendances ou anomalies dans les données de performance et les mettre en évidence sans sollicitation explicite (cf. module **Alertes**).
- Contribuer à la formulation de synthèses et de rapports (cf. module **Reporting**).

## 4. Limites

- ORION ne dispose que des données présentes sur la plateforme, dans le périmètre auquel l'utilisateur qui l'interroge a accès : il ne peut ni consulter, ni révéler une donnée hors de ce périmètre.
- ORION assiste la décision, il ne la prend jamais à la place de l'utilisateur : toute recommandation reste soumise au jugement du décideur.
- ORION ne peut produire une analyse fiable que sur des données elles-mêmes fiables ; la qualité de ses réponses dépend directement de la qualité des données intégrées à la plateforme (cf. module **Import**).
- ORION n'agit pas de façon autonome sur les systèmes externes à ORBITA AI : ses actions se limitent au périmètre de la plateforme.
- ORION reste soumis à l'ensemble des principes de sécurité et de confidentialité définis dans [SECURITY_PRINCIPLES.md](SECURITY_PRINCIPLES.md), notamment quant au périmètre de données qu'il peut mobiliser pour répondre à un utilisateur donné.

## 5. Positionnement d'ORION dans l'expérience produit

ORION incarne, plus que tout autre élément du logiciel, la promesse d'ORBITA AI définie dans [PROJECT_VISION.md](PROJECT_VISION.md) : transformer la donnée en décision. Son comportement, son ton et sa présence dans l'interface doivent rester conformes en tout point à la personnalité de marque définie dans le [BRAND_GUIDELINES.md](../branding/BRAND_GUIDELINES.md) : assurance, sobriété, clarté — jamais de familiarité excessive ni de posture ludique.
