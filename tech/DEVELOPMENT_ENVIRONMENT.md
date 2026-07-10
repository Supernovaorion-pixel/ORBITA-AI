# ORBITA AI — Development Environment

> Ce document définit l'environnement de développement officiel du projet, cohérent avec le socle fixé dans [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md).

## 1. IDE recommandé

- **Visual Studio** (édition complète) est l'IDE de référence pour le développement du socle applicatif C#/.NET, en raison de son intégration native avec l'outillage de diagnostic et de refactorisation .NET.
- **Visual Studio Code** est l'IDE de référence pour le développement de la couche Présentation (TypeScript/React), et constitue une alternative légère acceptée pour le backend sur les postes Linux et macOS.
- Le choix de l'IDE reste une préférence individuelle du contributeur ; seule la configuration partagée du projet (formatage, linting, cf. §4-5) est contraignante et commune à tout IDE utilisé.

## 2. Gestionnaire de dépendances

- **Backend (.NET)** — le gestionnaire de paquets natif de l'écosystème .NET gère l'ensemble des dépendances du socle applicatif, avec verrouillage explicite des versions pour garantir des builds reproductibles.
- **Frontend (TypeScript/React)** — un gestionnaire de paquets Node.js à verrouillage strict de version (fichier de verrouillage systématiquement commité) gère les dépendances de la couche Présentation.
- Aucune dépendance n'est installée manuellement en dehors de ces gestionnaires : toute dépendance du projet est déclarée et traçable (cf. [DEPENDENCY_POLICY.md](DEPENDENCY_POLICY.md)).

## 3. Environnement local

- Chaque contributeur dispose d'un environnement local reproductible incluant : le runtime .NET (version LTS alignée sur celle du projet), l'environnement Node.js correspondant à la version verrouillée du projet, et une instance locale de PostgreSQL (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md)) exécutée en conteneur pour éviter toute divergence de configuration entre postes.
- L'environnement local doit pouvoir démarrer l'ensemble de la plateforme (Core et modules natifs) sans dépendance à un service Cloud externe, condition nécessaire à un développement également possible en contexte On-Premise déconnecté.
- Un jeu de données de démonstration, représentatif mais non sensible, est fourni pour permettre le développement et la vérification sans donnée réelle d'Organisation cliente.

## 4. Formatage

- Un style de formatage unique est appliqué automatiquement à l'enregistrement du fichier, pour le backend comme pour le frontend, éliminant toute discussion de style lors des revues de code.
- Le formatage est vérifié en amont de toute fusion de code (cf. [GIT_WORKFLOW.md](GIT_WORKFLOW.md) §4) : un code mal formé n'est jamais fusionné, quelle que soit la qualité de son contenu.

## 5. Linting (analyse statique)

- Le backend est soumis à un ensemble de règles d'analyse statique du code (analyseurs natifs de l'écosystème .NET), configurées pour faire respecter les principes définis dans [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) (complexité, nommage, cf. [CODE_QUALITY.md](CODE_QUALITY.md)).
- Le frontend est soumis à un analyseur statique standard de l'écosystème TypeScript, garantissant la cohérence de style et la détection d'anti-patterns courants.
- Un avertissement de linting non résolu bloque la fusion au même titre qu'une erreur de formatage.

## 6. Documentation

- Toute interface publique d'un module (cf. [architecture/APPLICATION_LAYERS.md](../architecture/APPLICATION_LAYERS.md)) est documentée directement dans le code source via les conventions de documentation natives du langage (commentaires de documentation C#, annotations TSDoc pour TypeScript), permettant une génération automatique de documentation technique de référence.
- Cette documentation technique complète, sans jamais la remplacer, la documentation fonctionnelle et architecturale déjà établie dans [docs/](../docs/), [architecture/](../architecture/) et [features/](../features/).

## 7. Cohérence entre postes de développement

- L'ensemble de la configuration d'environnement (versions verrouillées, règles de formatage et de linting) est déclaré dans le dépôt de code, garantissant qu'un nouveau contributeur reproduit un environnement strictement identique dès son arrivée, sans configuration manuelle sujette à divergence.
- Cette reproductibilité est une condition de la maintenabilité à dix ans définie dans [architecture/TECHNICAL_SPECIFICATION.md](../architecture/TECHNICAL_SPECIFICATION.md) §1.
