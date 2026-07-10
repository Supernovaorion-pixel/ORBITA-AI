# ORBITA AI — Release Milestones

> Ce document définit les jalons officiels de publication du projet, de la première version interne jusqu'aux évolutions post-V1, en cohérence avec [tech/RELEASE_PROCESS.md](../tech/RELEASE_PROCESS.md) et [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md).

## 1. Alpha

- **Correspond à** : Phase 15 (cf. [DEVELOPMENT_PHASES.md](DEVELOPMENT_PHASES.md)).
- **Contenu** : intégralité du périmètre fonctionnel prévu pour la V1 (Phases 2 à 13), ayant passé les Tests globaux (Phase 14).
- **Public** : usage strictement interne, aucune diffusion à une Organisation cliente.
- **Sortie de jalon** : critères du canal Alpha satisfaits (cf. [tech/RELEASE_PROCESS.md](../tech/RELEASE_PROCESS.md) §3).

## 2. Beta

- **Correspond à** : Phase 16.
- **Contenu** : identique à l'Alpha, corrigé des anomalies qui y ont été détectées.
- **Public** : Organisations clientes volontaires, dans un cadre encadré et réversible.
- **Sortie de jalon** : absence d'anomalie bloquante remontée durant une période d'observation suffisante ; retours d'acceptation collectés conformément à [TEST_ACCEPTANCE.md](TEST_ACCEPTANCE.md) §5.

## 3. RC (Release Candidate)

- **Correspond à** : Phase 17.
- **Contenu** : périmètre fonctionnel gelé, aucune nouvelle fonctionnalité, uniquement des corrections d'anomalies bloquantes remontées en Beta.
- **Public** : vérification finale, diffusion élargie possible aux mêmes conditions que la Beta.
- **Sortie de jalon** : aucune anomalie bloquante ou de sécurité non résolue.

## 4. V1 (Version Stable)

- **Correspond à** : Phase 18.
- **Contenu** : première version officiellement commercialisable d'ORBITA AI, couvrant l'intégralité des quatre éditions (Community à Enterprise) et des deux modes de déploiement (Cloud, On-Premise), conformément à [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md).
- **Public** : disponibilité générale, toute Organisation cliente.
- **Signification** : ce jalon marque la fin du présent plan de développement (cf. [MASTER_DEVELOPMENT_PLAN.md](MASTER_DEVELOPMENT_PLAN.md) §5).

## 5. V1.1

- **Nature** : première évolution mineure post-V1 (incrément MINEUR ou CORRECTIF, cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §2).
- **Contenu attendu** : corrections d'anomalies identifiées en production, ajustements mineurs issus des retours des premières Organisations clientes en V1, sans rupture de compatibilité.
- **Processus** : suit le même processus de publication que la V1 (cf. [tech/RELEASE_PROCESS.md](../tech/RELEASE_PROCESS.md)), à périmètre réduit et sur un cycle plus court.
- **Planification détaillée** : établie séparément une fois la V1 publiée et les premiers retours de production disponibles, selon les mêmes principes de rigueur que ceux définis dans ce dossier.

## 6. V2

- **Nature** : évolution majeure post-V1, pouvant inclure des fonctionnalités identifiées dans [features/FUTURE_FEATURES.md](../features/FUTURE_FEATURES.md).
- **Contenu** : non fixé à ce stade ; toute fonctionnalité candidate à la V2 doit, avant sa planification, faire l'objet d'une spécification fonctionnelle formelle (cf. [features/FUTURE_FEATURES.md](../features/FUTURE_FEATURES.md) §9) et d'une vérification de cohérence architecturale, exactement selon le même processus que celui ayant produit la V1.
- **Positionnement** : une évolution majeure (V2) peut introduire un incrément MAJEUR de version si elle implique une rupture de compatibilité ascendante, auquel cas elle est traitée avec la rigueur définie dans [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §2 et §8.
- **Portée de ce document** : la V2 n'est pas planifiée en détail ici ; ce document ne fait que la positionner comme le jalon suivant la stabilisation de la V1 et de ses versions mineures.

## 7. Principe transversal

Aucun jalon n'est atteint par anticipation de calendrier : chaque jalon correspond à la clôture effective et vérifiée des phases et critères qui le précèdent (cf. [QUALITY_GATES.md](QUALITY_GATES.md)). Un jalon annoncé mais non vérifié selon ces critères n'est pas considéré comme atteint.
