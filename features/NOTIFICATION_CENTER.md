# ORBITA AI — Feature Specification: Notification Center

> Ce document spécifie le comportement fonctionnel complet du Notification Center (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §15), accessible depuis la Topbar de tout écran (cf. [ux/NAVIGATION.md](../ux/NAVIGATION.md) §3).

## 1. Objectif

Acheminer vers chaque utilisateur les notifications pertinentes à son rôle et son périmètre, qu'elles proviennent des Alertes, des Rapports planifiés, des Imports, des Licences ou de l'activité d'Administration.

## 2. Notifications

Catégories couvertes :
- **Alertes métier** (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)) — écarts, anomalies, opportunités.
- **Rapports** — rapport planifié généré avec succès ou en échec (cf. [REPORTING_ENGINE.md](REPORTING_ENGINE.md) §5).
- **Imports** — achèvement ou échec d'un Import, notamment ceux réalisés via Connecteur (cf. [CONNECTORS.md](CONNECTORS.md) §7).
- **Licence** — approche d'échéance, dépassement de volume (cf. [LICENSE_MANAGEMENT.md](LICENSE_MANAGEMENT.md) §7, §9).
- **Administration** — événements relatifs aux utilisateurs (ex. invitation acceptée) pertinents pour un Administrateur.

## 3. Priorités

- Chaque notification hérite du niveau de priorité de l'événement qui l'a générée (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §4 pour les alertes métier).
- Les notifications critiques sont mises en avant visuellement et ne disparaissent pas automatiquement tant qu'elles n'ont pas été lues ou traitées.
- Les notifications informatives (ex. rapport généré avec succès) sont présentées de façon discrète et peuvent s'effacer automatiquement après consultation.

## 4. Lecture

- Une notification non lue est visuellement distincte d'une notification déjà consultée.
- La consultation du détail d'une notification (ex. ouverture de l'alerte associée) la marque automatiquement comme lue, sans action supplémentaire requise.
- Le compteur de notifications non lues, affiché en Topbar, reflète en continu l'état réel des notifications de l'utilisateur.

## 5. Archivage

- Une notification lue peut être archivée manuellement, ou automatiquement après un délai raisonnable pour les catégories non critiques.
- L'archivage d'une notification n'affecte jamais l'état de l'élément qui l'a générée (ex. archiver une notification d'alerte ne modifie pas l'état de l'alerte elle-même dans [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §7, qui suit son propre cycle).
- Les notifications archivées restent consultables dans un historique dédié.

## 6. Canaux

- Les notifications sont présentées dans le centre de notifications intégré à la plateforme (canal principal), consultable depuis tout écran.
- Selon la configuration de l'utilisateur (cf. [SETTINGS.md](SETTINGS.md) §3.4), certaines catégories de notifications peuvent également être relayées par un canal complémentaire (ex. notification par courrier électronique), sans que cela ne remplace jamais leur disponibilité dans le centre de notifications intégré.

## 7. Préférences utilisateur

- Chaque utilisateur configure les catégories de notifications qu'il souhaite recevoir activement, à l'exception des alertes critiques relevant strictement de son périmètre de responsabilité, qui restent toujours acheminées (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §8).

## 8. Cohérence avec ORION

- Les résumés périodiques produits par ORION (quotidien, hebdomadaire, mensuel — cf. [ORION.md](ORION.md) §11-13) constituent un canal de synthèse complémentaire au Notification Center, sans dupliquer le détail de chaque notification individuelle déjà accessible depuis celui-ci.
