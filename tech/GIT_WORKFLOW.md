# ORBITA AI — Git Workflow

> Ce document définit le flux de travail Git officiel du projet, s'appuyant sur les conventions de nommage définies dans [architecture/NAMING_CONVENTIONS.md](../architecture/NAMING_CONVENTIONS.md) §9-10.

## 1. Branches

- **`main`** — branche stable, reflétant en permanence l'état actuellement publié ou publiable de la version **Stable** la plus récente (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)).
- **Branches de fonctionnalité** (`feature/...`) — créées depuis `main`, dédiées au développement d'une fonctionnalité définie dans [features/](../features/), fusionnées dans `main` une fois vérifiées.
- **Branches de correction** (`fix/...`) — créées depuis `main`, dédiées à la correction d'une anomalie.
- **Branches de ligne LTS** (`release/lts-X.Y`, cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §4) — branches de maintenance longue durée, recevant exclusivement des corrections rétrocompatibles, jamais de nouvelle fonctionnalité.
- Aucun développement n'est réalisé directement sur `main` : toute évolution transite par une branche dédiée et une revue avant fusion.

## 2. Commits

- Chaque commit suit le format défini dans [architecture/NAMING_CONVENTIONS.md](../architecture/NAMING_CONVENTIONS.md) §10 : `<type>: <résumé au présent>`.
- Un commit correspond à un changement cohérent et unique ; un commit qui mélange une fonctionnalité et une correction sans lien est scindé avant fusion.
- L'historique de `main` reste linéaire et lisible : les commits intermédiaires de mise au point (corrections d'une revue en cours) sont consolidés avant fusion finale, pour qu'un commit sur `main` corresponde toujours à un changement complet et cohérent.

## 3. Pull Requests

- Toute branche de fonctionnalité ou de correction est proposée à la fusion via une Pull Request, jamais fusionnée directement.
- Une Pull Request référence explicitement la spécification fonctionnelle concernée (cf. [features/](../features/)) ou l'anomalie corrigée, conformément à l'exigence de documentation avant développement (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §4).
- Une Pull Request qui introduit une dépendance nouvelle doit justifier son ajout au regard de [DEPENDENCY_POLICY.md](DEPENDENCY_POLICY.md) §2.

## 4. Validation avant fusion

Aucune Pull Request n'est fusionnée sans que l'ensemble des conditions suivantes soit satisfait :

1. Revue approuvée par au moins un contributeur autre que l'auteur (cf. [CODE_QUALITY.md](CODE_QUALITY.md) §7).
2. Conformité au formatage et à l'analyse statique (cf. [DEVELOPMENT_ENVIRONMENT.md](DEVELOPMENT_ENVIRONMENT.md) §4-5).
3. Ensemble des tests de non-régression exécutés avec succès (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md) §7).
4. Absence de dépendance circulaire introduite entre modules (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) §5).
5. Documentation associée mise à jour si le changement affecte un comportement déjà documenté (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §4).

Une seule condition non satisfaite suffit à bloquer la fusion, sans exception ponctuelle.

## 5. Release (publication de version)

- Une publication de version est réalisée depuis `main`, une fois l'ensemble des fonctionnalités prévues pour cette version vérifiées (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md)) et le canal de maturité approprié atteint (Alpha, Beta, RC, Stable — cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §3).
- Chaque publication est associée à une étiquette de version explicite (Semantic Versioning) et à des notes de version résumant les changements, destinées aux Organisations clientes (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)).

## 6. Hotfix (correction urgente)

- Une correction urgente affectant une version déjà publiée (Stable ou LTS) est développée sur une branche dédiée (`fix/...`) créée directement depuis la version concernée (`main` ou la branche `release/lts-X.Y` appropriée), jamais depuis une branche de fonctionnalité en cours.
- Un hotfix suit exactement les mêmes exigences de validation que toute autre fusion (§4), sans dispense de revue ou de test au prétexte de son urgence.
- Un hotfix appliqué à une ligne LTS est également reporté (cherry-pick) vers `main` si le défaut corrigé y est également présent, afin d'éviter sa réapparition dans une publication future.

## 7. Principe transversal

L'historique Git constitue une source de vérité complémentaire à la documentation du projet, jamais une source concurrente : toute divergence entre l'intention documentée (cf. [docs/](../docs/), [architecture/](../architecture/), [features/](../features/)) et l'historique réel du code doit être résolue en faveur d'une mise à jour explicite de la documentation, jamais laissée en l'état.
