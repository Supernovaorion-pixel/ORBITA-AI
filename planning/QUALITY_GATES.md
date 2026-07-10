# ORBITA AI — Quality Gates

> Ce document définit les critères obligatoires devant être satisfaits avant tout passage d'une phase à la suivante (cf. [DEVELOPMENT_PHASES.md](DEVELOPMENT_PHASES.md)). Une phase dont un critère n'est pas satisfait reste ouverte, quelle que soit la pression de calendrier.

## 1. Portail commun à toute phase

Avant qu'une phase ne soit déclarée close, l'ensemble des conditions suivantes doit être vérifié :

1. **Tous les modules de la phase respectent la [DEFINITION_OF_DONE.md](DEFINITION_OF_DONE.md)**, sans exception.
2. **Tous les tests de non-régression existants réussissent**, incluant ceux hérités des phases précédentes (cf. [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) §7).
3. **Aucune anomalie bloquante ouverte** sur le périmètre de la phase.
4. **Aucune dépendance circulaire ou interdite** introduite (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) §5).
5. **La documentation de conception reste cohérente** avec ce qui a été implémenté ; toute clarification nécessaire a été reportée dans les documents source (cf. [DEVELOPMENT_RULES.md](DEVELOPMENT_RULES.md) §8).
6. **Les critères d'acceptation définis dans [TEST_ACCEPTANCE.md](TEST_ACCEPTANCE.md)** pour le périmètre de la phase sont satisfaits.

## 2. Critères spécifiques par phase

### Phase 1 — Fondations techniques
Environnement reproductible sur un poste neuf sans intervention manuelle non documentée (cf. [tech/DEVELOPMENT_ENVIRONMENT.md](../tech/DEVELOPMENT_ENVIRONMENT.md) §7).

### Phase 2 — Core
Cloisonnement par Organisation vérifié par test d'intégration dédié ; aucune fuite de donnée possible entre Organisations, même simulées.

### Phase 3 — Import Engine
Ensemble des scénarios d'anomalie définis dans [features/IMPORT_ENGINE.md](../features/IMPORT_ENGINE.md) §8 rejoués et correctement gérés.

### Phase 4 — Analytics Engine
Temps de calcul conformes à [tech/PERFORMANCE_TARGETS.md](../tech/PERFORMANCE_TARGETS.md) §3 sur volumétrie représentative.

### Phase 5 — Dashboard
Conformité totale à [ux/ACCESSIBILITY_UI.md](../ux/ACCESSIBILITY_UI.md) sur l'ensemble des cartes et graphiques livrés ; cohérence comportementale des 12 cartes KPI vérifiée (cf. [features/DASHBOARD.md](../features/DASHBOARD.md) §2).

### Phase 6 — Forecast
Toute prévision affichée est accompagnée d'un intervalle de confiance ; aucune projection présentée comme une certitude (cf. [features/FORECAST_ENGINE.md](../features/FORECAST_ENGINE.md) §9).

### Phase 7 — Reporting
Fidélité de contenu vérifiée entre les formats PDF, Excel et PowerPoint pour chaque modèle de rapport du catalogue officiel.

### Phase 8 — ORION
Aucune réponse d'ORION non traçable à une donnée réelle ; aucun accès d'ORION à une donnée hors du périmètre de l'utilisateur testé.

### Phase 9 — Administration
Matrice de droits d'accès testée pour l'intégralité des rôles définis dans [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md) §4.

### Phase 10 — Licences
Chacune des quatre éditions (Community à Enterprise) testée isolément, confirmant l'activation exacte du périmètre défini dans [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md) ; rôle Product Owner vérifié comme non restreignable par une licence cliente (cf. [PRODUCT_OWNER_RULES.md](PRODUCT_OWNER_RULES.md)).

### Phase 11 — Plugins
Un plugin de référence installé, activé, désactivé et désinstallé sans impact résiduel sur le noyau ni sur d'autres plugins.

### Phase 12 — Connecteurs
Synchronisation ERP et CRM de référence vérifiée en mode incrémental sans duplication ni perte de donnée.

### Phase 13 — Optimisations
Intégralité des cibles de [tech/PERFORMANCE_TARGETS.md](../tech/PERFORMANCE_TARGETS.md) satisfaites sur volumétrie Enterprise complète ; exercice de restauration réel réalisé avec succès (cf. [tech/BACKUP_AND_RECOVERY.md](../tech/BACKUP_AND_RECOVERY.md) §2).

### Phase 14 — Tests globaux
Couverture complète de [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) exécutée sans échec sur l'ensemble du système assemblé.

### Phase 15 — Version Alpha
Critères du canal Alpha définis dans [tech/RELEASE_PROCESS.md](../tech/RELEASE_PROCESS.md) §3 satisfaits.

### Phase 16 — Version Beta
Absence d'anomalie bloquante remontée par les Organisations volontaires sur une durée d'observation suffisante.

### Phase 17 — Release Candidate
Aucune anomalie bloquante ou de sécurité non résolue ; aucune nouvelle fonctionnalité introduite depuis le gel du périmètre.

### Phase 18 — Version Stable
Intégralité des critères du canal Stable satisfaite ; jalon V1 formellement déclaré (cf. [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md)).

## 3. Autorité de validation

La clôture d'une phase et l'ouverture de la suivante est une décision explicite, jamais un effet automatique du seul dépôt de code. Cette décision est prise au regard des seuls critères objectifs de ce document, jamais sur la base d'une pression de calendrier commercial.
