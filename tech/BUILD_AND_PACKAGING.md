# ORBITA AI — Build and Packaging

> Ce document définit le processus de construction et de distribution du logiciel, cohérent avec le socle défini dans [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md) et les modes de déploiement définis dans [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §5.

## 1. Construction (build)

- Le backend (.NET) est compilé en un ensemble d'assemblies exécutables, un par module natif (cf. [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md)), assemblés par le projet `Host` en une application unique pour le déploiement en processus unique.
- Le frontend (TypeScript/React) est transformé en un ensemble de ressources statiques optimisées (regroupement, minification), servies indépendamment du backend.
- La construction est intégralement automatisée et reproductible : deux constructions réalisées à partir du même code source produisent un résultat strictement identique, condition nécessaire à la confiance dans le processus de publication (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)).

## 2. Installation

- **Cloud** — l'installation ne concerne pas l'Organisation cliente : la plateforme est déployée et exploitée pour son compte (cf. [DEPLOYMENT_STRATEGY.md](DEPLOYMENT_STRATEGY.md)).
- **On-Premise Windows (priorité)** — un programme d'installation natif guide l'Administrateur de l'Organisation à travers la configuration initiale (connexion à la base PostgreSQL, cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md), paramètres réseau) et l'enregistrement du logiciel comme service démarrant automatiquement.
- **On-Premise Linux (prévu)** — l'installation s'appuie sur une image de conteneur standardisée, garantissant un comportement identique à celui de l'installateur Windows sans nécessiter de programme d'installation natif distinct.
- **On-Premise macOS (prévu)** — non prioritaire pour un déploiement serveur (macOS n'étant pas un système d'exploitation serveur d'entreprise usuel) ; l'accès à la plateforme depuis un poste macOS s'effectue par navigateur, sans installation locale requise (cf. §5).

## 3. Packaging

- **Backend** — empaqueté sous forme d'image de conteneur standardisée (format ouvert, exécutable indifféremment sur Windows, Linux ou macOS via un moteur de conteneurisation compatible), constituant l'unité de distribution de référence pour le Cloud et pour l'On-Premise Linux.
- **Frontend** — empaqueté sous forme de ressources statiques versionnées, distribuées avec le backend ou séparément selon l'infrastructure de l'Organisation cliente.
- **Installateur Windows** — empaquette l'image de conteneur ou l'exécutable natif backend, les ressources frontend, et le programme d'installation guidé (§2), en un seul programme d'installation autonome.
- Un seul et même artefact de construction (§1) alimente l'ensemble de ces formats de packaging : aucun code n'est recompilé différemment selon la cible de distribution.

## 4. Signature

- Tout artefact distribué (installateur Windows, image de conteneur) est signé numériquement par l'éditeur avant publication, garantissant à l'Organisation cliente l'authenticité et l'intégrité du logiciel installé.
- La vérification de signature est une étape bloquante du processus d'installation et de mise à jour (cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) §1) : un artefact non signé ou dont la signature ne correspond pas n'est jamais installé.
- Les clés de signature sont gérées selon les principes définis dans [SECRET_MANAGEMENT.md](SECRET_MANAGEMENT.md).

## 5. Distribution

- **Cloud** — l'artefact est déployé directement sur l'infrastructure d'exploitation, sans distribution à l'Organisation cliente (cf. [DEPLOYMENT_STRATEGY.md](DEPLOYMENT_STRATEGY.md) §2).
- **On-Premise** — l'artefact est mis à disposition de l'Organisation cliente via un espace de téléchargement sécurisé, accessible selon sa licence active (cf. [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md)).
- **Accès utilisateur final** — quel que soit le mode de déploiement, l'utilisateur final accède à la plateforme via un navigateur web standard, sans installation locale de sa part, garantissant un accès identique depuis Windows, Linux ou macOS (cf. [ux/RESPONSIVE_RULES.md](../ux/RESPONSIVE_RULES.md)).

## 6. Principe transversal

Le packaging ne doit jamais introduire de divergence fonctionnelle entre éditions ou modes de déploiement : la même construction (§1) sert Community, Starter, Business et Enterprise, Cloud et On-Premise ; seule la configuration de licence (cf. [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md)) et de déploiement (cf. [CONFIGURATION_MANAGEMENT.md](CONFIGURATION_MANAGEMENT.md)) diffère.
