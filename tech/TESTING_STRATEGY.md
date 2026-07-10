# ORBITA AI — Testing Strategy

> Ce document définit la stratégie officielle de vérification du logiciel, conforme à l'exigence de [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §9 : aucune évolution n'est fusionnée sans avoir été préalablement vérifiée.

## 1. Principe général — pyramide de tests

La stratégie de test suit une pyramide, la base la plus large étant la moins coûteuse et la plus rapide à exécuter :

```
            Tests de non-régression (continus)
        Tests de performance (ciblés, réguliers)
     Tests UI (parcours critiques)
  Tests fonctionnels (comportement métier de bout en bout)
Tests d'intégration (interaction entre modules)
Tests unitaires (base — la majorité des tests)
```

## 2. Tests unitaires

- Couvrent la logique du Domaine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)) et de l'Application, module par module, en isolation complète des autres modules et de toute infrastructure externe (base de données, connecteur).
- Constituent la majorité du volume de tests du projet, exécutés systématiquement et rapidement à chaque changement de code, y compris localement avant toute proposition de fusion.
- Toute règle métier définie dans [features/](../features/) doit être couverte par un test unitaire correspondant, conformément au seuil de couverture défini dans [CODE_QUALITY.md](CODE_QUALITY.md) §5.

## 3. Tests d'intégration

- Vérifient le comportement correct des interactions entre un module et l'infrastructure dont il dépend réellement (base de données PostgreSQL, cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md)), ainsi que les interactions inter-modules autorisées par [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md).
- Exécutés dans un environnement reproductible et isolé (cf. [DEVELOPMENT_ENVIRONMENT.md](DEVELOPMENT_ENVIRONMENT.md) §3), jamais contre une donnée réelle d'Organisation cliente.
- Vérifient notamment le bon fonctionnement du système d'événements (cf. [architecture/EVENT_SYSTEM.md](../architecture/EVENT_SYSTEM.md)) entre modules émetteurs et abonnés.

## 4. Tests fonctionnels

- Vérifient qu'un comportement métier complet, tel que décrit dans [features/](../features/) (ex. un cycle complet d'Import, cf. [features/IMPORT_ENGINE.md](../features/IMPORT_ENGINE.md)), se déroule conformément à sa spécification, de bout en bout à travers les couches Application et Domaine.
- Servent de garantie que la spécification fonctionnelle officielle reste respectée au fil des évolutions du code, indépendamment de son implémentation interne.

## 5. Tests UI

- Vérifient les parcours utilisateurs critiques définis dans [ux/USER_FLOWS.md](../ux/USER_FLOWS.md) (ex. connexion → Dashboard, investigation d'une alerte), au niveau de la couche Présentation.
- Se concentrent sur les parcours à fort enjeu plutôt que sur une couverture exhaustive de chaque écran, pour un rapport coût/bénéfice maîtrisé (ces tests étant les plus coûteux à maintenir).
- Vérifient également le respect des règles d'accessibilité définies dans [ux/ACCESSIBILITY_UI.md](../ux/ACCESSIBILITY_UI.md) sur les parcours couverts.

## 6. Tests de performance

- Vérifient que les objectifs chiffrés définis dans [PERFORMANCE_TARGETS.md](PERFORMANCE_TARGETS.md) sont respectés sur des volumes de données représentatifs d'une Organisation Enterprise à pleine échelle (cf. [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) §7).
- Exécutés à un rythme régulier (à chaque changement significatif touchant l'Import Engine, l'Analytics Engine ou le Forecast Engine, et systématiquement avant chaque publication de version, cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)), pour détecter toute régression de performance avant qu'elle n'atteigne un environnement de production.

## 7. Tests de non-régression

- Rejouent l'ensemble des scénarios déjà validés (unitaires, intégration, fonctionnels, UI critiques) à chaque changement proposé, garantissant qu'une évolution nouvelle ne casse pas un comportement déjà validé.
- Constituent la porte de vérification systématique avant toute fusion (cf. [GIT_WORKFLOW.md](GIT_WORKFLOW.md) §4) et avant toute publication de version (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)).

## 8. Environnements de test

- Les tests unitaires et d'intégration s'exécutent dans l'environnement de développement local et à chaque proposition de changement, avant fusion.
- Les tests fonctionnels et UI s'exécutent dans un environnement dédié reproduisant fidèlement la configuration cible (Cloud et On-Premise), avant chaque publication de version.
- Les tests de performance s'exécutent dans un environnement dimensionné pour représenter une charge Enterprise réelle, distinct de l'environnement de vérification fonctionnelle courant.

## 9. Principe transversal

Aucune fonctionnalité définie dans [features/](../features/) n'est considérée comme terminée sans les tests correspondant à son niveau de criticité. Un changement qui n'ajoute ou n'ajuste aucun test n'est jamais fusionné sur la seule base d'une vérification manuelle non reproductible.
