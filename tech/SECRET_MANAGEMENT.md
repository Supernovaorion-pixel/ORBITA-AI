# ORBITA AI — Secret Management

> Ce document définit la gestion technique des secrets (identifiants, clés, jetons) utilisés par la plateforme, en application des principes de confidentialité définis dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §1.

## 1. Périmètre

Sont considérés comme secrets : les identifiants de connexion à la base de données (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md)), les clés de signature des artefacts (cf. [BUILD_AND_PACKAGING.md](BUILD_AND_PACKAGING.md) §4), les identifiants d'accès aux fournisseurs d'intelligence artificielle (cf. [AI_INTEGRATION.md](AI_INTEGRATION.md)), les identifiants de connexion aux systèmes externes via les Connecteurs (cf. [features/CONNECTORS.md](../features/CONNECTORS.md)), et les jetons d'authentification des utilisateurs et des Organisations.

## 2. Stockage

- Aucun secret n'est jamais stocké en texte clair, ni dans le code source, ni dans un fichier de configuration versionné, ni dans une entrée de journal (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §4).
- Les secrets sont stockés dans un emplacement dédié, chiffré au repos, distinct du reste de la configuration applicative (cf. [CONFIGURATION_MANAGEMENT.md](CONFIGURATION_MANAGEMENT.md)).
- **En Cloud**, cet emplacement est un service de gestion de secrets dédié de l'infrastructure d'exploitation, isolé par Organisation cliente lorsque le secret est propre à celle-ci (ex. identifiants d'un Connecteur).
- **En On-Premise**, cet emplacement est un coffre de secrets local chiffré, dont l'accès est restreint au compte technique d'exécution de la plateforme, jamais accessible directement par un utilisateur applicatif, y compris Administrateur.

## 3. Rotation

- Les secrets techniques (clés de signature, identifiants système) sont soumis à une politique de rotation périodique, planifiée et documentée, sans interruption de service lors du changement.
- Les secrets propres à une Organisation (identifiants de Connecteur) peuvent être renouvelés à tout moment par l'Administrateur habilité, l'ancien secret étant immédiatement invalidé dès l'enregistrement du nouveau.
- Une rotation de secret est toujours journalisée (qui, quand — jamais la valeur du secret lui-même), conformément à [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3.

## 4. Protection

- Les secrets ne transitent jamais en clair sur le réseau : tout échange impliquant un secret est chiffré en transit (cf. [SECURITY_REQUIREMENTS.md](SECURITY_REQUIREMENTS.md) §4).
- Un secret n'est jamais journalisé, y compris dans un journal de diagnostic (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §4), ni affiché en clair dans l'interface une fois enregistré (une valeur masquée est présentée à la place, avec une action explicite de remplacement plutôt que de consultation).
- Une tentative d'accès à un secret hors du contexte technique autorisé (ex. tentative de lecture directe du coffre par un processus non habilité) est considérée comme un incident de sécurité (cf. [SECURITY_REQUIREMENTS.md](SECURITY_REQUIREMENTS.md)).

## 5. Accès

- L'accès à un secret est strictement limité au composant technique qui en a l'usage fonctionnel direct (ex. seul l'Import Engine accède aux identifiants d'un Connecteur qu'il exploite), jamais partagé largement par commodité.
- Aucun utilisateur humain, y compris Administrateur de l'Organisation ou personnel d'exploitation, ne dispose d'un accès permanent en lecture aux secrets techniques du système ; un accès exceptionnel (diagnostic approfondi) est temporaire, tracé et justifié.
- Le cloisonnement par Organisation (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4) s'applique strictement aux secrets propres à chaque Organisation : aucun secret d'une Organisation n'est jamais accessible dans le contexte d'exécution d'une autre.

## 6. Cas particulier — mode On-Premise

- En mode On-Premise, l'Organisation cliente conserve la maîtrise physique du coffre de secrets local ; l'éditeur d'ORBITA AI n'a, dans ce mode, aucun accès aux secrets propres à cette Organisation, conformément à l'objectif de gouvernance renforcée recherché par ce mode de déploiement (cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §5).

## 7. Principe transversal

La gestion des secrets est conçue selon le principe du moindre privilège et de la moindre exposition : un secret n'existe en clair qu'au moment strictement nécessaire de son utilisation technique, jamais au-delà.
