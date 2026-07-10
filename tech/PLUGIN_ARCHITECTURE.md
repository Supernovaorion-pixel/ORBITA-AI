# ORBITA AI — Plugin Architecture

> Ce document définit l'implémentation technique du Plugin System (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §13, [features/PLUGIN_SYSTEM.md](../features/PLUGIN_SYSTEM.md)), sur la base du socle défini dans [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md).

## 1. Principe

Un plugin est un module de code indépendant, chargé dynamiquement par la plateforme, s'intégrant exclusivement via les points d'extension exposés par les modules natifs (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §2). Un plugin n'est jamais compilé au sein du noyau applicatif : il reste un artefact distinct, chargé à l'exécution.

## 2. Cycle de vie

1. **Publication** — un plugin est publié sous forme d'un artefact autonome, conforme au contrat technique du point d'extension qu'il implémente.
2. **Installation** — l'artefact est déposé dans l'environnement de la plateforme (cf. [features/PLUGIN_SYSTEM.md](../features/PLUGIN_SYSTEM.md) §4), sans redémarrage de la plateforme dans le cas général.
3. **Chargement** — la plateforme charge dynamiquement le plugin dans un contexte d'exécution isolé (§4).
4. **Activation** — le plugin devient opérationnel et commence à répondre aux sollicitations du point d'extension auquel il est rattaché.
5. **Désactivation** — le plugin cesse de répondre, sans être déchargé de l'environnement (redémarrage rapide possible).
6. **Désinstallation** — l'artefact est retiré de l'environnement, ses données propres étant conservées ou supprimées selon le choix explicite de l'Administrateur.

## 3. Chargement

- Le chargement d'un plugin s'appuie sur les mécanismes natifs de chargement dynamique de l'écosystème .NET, permettant l'ajout et le retrait d'un plugin sans recompilation ni redéploiement du noyau applicatif.
- Un plugin déclare explicitement le point d'extension auquel il se rattache (ex. point d'extension de l'Import Engine pour un Connecteur) et la version d'interface qu'il implémente ; un plugin déclarant une version d'interface incompatible avec la version courante du noyau n'est pas chargé, avec un message explicite à l'Administrateur plutôt qu'un chargement partiel ou instable.

## 4. Isolation

- Chaque plugin s'exécute dans un contexte isolé du noyau applicatif et des autres plugins : une défaillance au sein d'un plugin (erreur non gérée, ressource indisponible) n'affecte jamais le fonctionnement du noyau ni des autres plugins actifs (cf. [architecture/ERROR_HANDLING.md](../architecture/ERROR_HANDLING.md) §5).
- Un plugin ne peut jamais accéder à la mémoire ou à l'état interne d'un autre plugin ou du noyau au-delà de ce que le point d'extension expose explicitement.

## 5. Signature

- Tout artefact de plugin est signé numériquement par son éditeur avant publication, selon les mêmes principes que ceux définis dans [BUILD_AND_PACKAGING.md](BUILD_AND_PACKAGING.md) §4.
- La plateforme vérifie la signature de tout plugin avant son chargement ; un plugin non signé ou dont la signature est invalide n'est jamais chargé, sans exception, y compris en environnement de développement au-delà d'un mode explicitement dédié à cet usage.
- La liste des signatures d'éditeurs de confiance est gérée au niveau de la configuration système (cf. [CONFIGURATION_MANAGEMENT.md](CONFIGURATION_MANAGEMENT.md) §2).

## 6. Permissions

- Un plugin déclare explicitement, avant son installation, le périmètre de données et de capacités qu'il requiert (ex. accès en lecture aux données Client pour un connecteur CRM), présenté à l'Administrateur pour validation avant activation (cf. [features/PLUGIN_SYSTEM.md](../features/PLUGIN_SYSTEM.md) §4).
- Un plugin n'obtient jamais d'accès au-delà de ce périmètre déclaré et validé ; toute tentative d'accès hors périmètre est bloquée et journalisée comme un incident de sécurité (cf. [SECURITY_REQUIREMENTS.md](SECURITY_REQUIREMENTS.md)).
- Les permissions d'un plugin sont soumises au même cloisonnement par Organisation que le reste de la plateforme (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4) : un plugin activé pour une Organisation n'a jamais accès aux données d'une autre.

## 7. Compatibilité

- Un plugin déclare la ou les versions du noyau applicatif avec lesquelles il est compatible ; la plateforme refuse le chargement d'un plugin incompatible avec sa version courante plutôt que de tenter un chargement partiel à risque.
- Une mise à jour du noyau qui modifierait un contrat d'interface exposé aux plugins constitue un changement MAJEUR (cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §7) ; l'Update Manager (cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) §6) signale explicitement tout plugin qui deviendrait incompatible avant application de la mise à jour.

## 8. Restriction par licence

- La disponibilité du chargement de plugins tiers dépend de l'offre de licence de l'Organisation (cf. [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md)), vérifiée par le module Licensing avant toute activation (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md)).
