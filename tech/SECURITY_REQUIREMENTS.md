# ORBITA AI — Security Requirements

> Ce document décline en exigences techniques concrètes les principes définis dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md), pour l'ensemble des éditions et modes de déploiement.

## 1. Authentification

- Chaque utilisateur s'authentifie individuellement avant tout accès à la plateforme ; aucun compte partagé n'est admis.
- Le mécanisme d'authentification supporte, au minimum pour les éditions Business et Enterprise, une authentification à facteurs multiples et une fédération d'identité avec le système d'authentification propre à l'Organisation cliente, sans imposer cette complexité aux éditions Community et Starter.
- Une session utilisateur inactive est automatiquement close après une durée configurable, raisonnable par défaut (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §6).
- Tout échec d'authentification répété déclenche une protection contre les tentatives automatisées, sans jamais bloquer définitivement un utilisateur légitime sans voie de recours.

## 2. Autorisation

- Toute action sur la plateforme est vérifiée au regard des permissions de l'utilisateur (cf. [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md) §5) avant exécution, jamais après.
- La vérification d'autorisation est appliquée de façon centralisée au niveau du module Security (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §11), jamais dupliquée ni réimplémentée localement par chaque module fonctionnel.
- Le cloisonnement par Organisation (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4) est vérifié à ce même niveau central, garantissant qu'aucune requête ne peut techniquement traverser la frontière d'une Organisation.

## 3. Journalisation

- Toute authentification, tentative d'accès refusée, et action administrative est journalisée conformément à [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3, de façon immuable.
- Les journaux de sécurité sont conservés séparément des journaux techniques généraux, avec un accès restreint aux profils habilités (cf. [features/AUDIT_AND_HISTORY.md](../features/AUDIT_AND_HISTORY.md)).

## 4. Chiffrement

- **En transit** — toute communication entre la Présentation et l'Application, entre l'Application et la base de données, et avec tout système externe (Connecteurs, fournisseurs d'intelligence artificielle) est chiffrée, sans exception, y compris pour un déploiement On-Premise sur réseau interne.
- **Au repos** — les données stockées dans la base PostgreSQL (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md)) et les sauvegardes (cf. [BACKUP_AND_RECOVERY.md](BACKUP_AND_RECOVERY.md)) sont chiffrées ; les secrets bénéficient d'une protection renforcée complémentaire (cf. [SECRET_MANAGEMENT.md](SECRET_MANAGEMENT.md)).
- Les algorithmes et protocoles de chiffrement retenus sont des standards ouverts et éprouvés, révisés périodiquement pour rester alignés sur l'état de l'art, jamais un mécanisme propriétaire non audité.

## 5. Protection des données

- Le cloisonnement strict par Organisation (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4) est une garantie technique vérifiée à chaque accès, pas seulement une convention de conception.
- Toute donnée exportée (cf. [features/EXPORT_ENGINE.md](../features/EXPORT_ENGINE.md)) est soumise aux mêmes vérifications d'autorisation que sa consultation à l'écran.
- Une Organisation cliente peut demander l'export ou la suppression complète de ses données, conformément à ses obligations réglementaires propres (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §7) ; cette opération est techniquement complète, sans reliquat de donnée oublié dans un composant annexe (journal excepté, conservé selon sa politique de rétention propre, cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §7).

## 6. Vérification continue

- La conformité à ces exigences est vérifiée à chaque publication de version (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)), incluant une revue des dépendances tierces au regard de vulnérabilités connues (cf. [DEPENDENCY_POLICY.md](DEPENDENCY_POLICY.md) §5).
- Toute vulnérabilité de sécurité identifiée, quelle que soit sa source (code propre, dépendance tierce), est traitée en priorité absolue, au besoin par une publication hors cycle standard (cf. [GIT_WORKFLOW.md](GIT_WORKFLOW.md) §6 — Hotfix).

## 7. Principe transversal

La sécurité est vérifiée à la conception de chaque fonctionnalité (cf. [architecture/TECHNICAL_SPECIFICATION.md](../architecture/TECHNICAL_SPECIFICATION.md) §6), jamais ajoutée après coup. Aucune fonctionnalité définie dans [features/](../features/) n'est considérée comme prête à l'implémentation sans que ses implications de sécurité aient été explicitement examinées au regard de ce document.
