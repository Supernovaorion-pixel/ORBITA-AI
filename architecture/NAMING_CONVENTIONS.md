# ORBITA AI — Naming Conventions

> Ce document définit les conventions de nommage officielles du projet. Elles s'appliquent indépendamment de tout langage ou technologie retenu ultérieurement, et garantissent la cohérence et la lisibilité du système sur le long terme (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §7).

## 1. Principe général

Tout nom utilisé dans le projet — fichier, classe, fonction, variable, module — doit :
- refléter un concept du vocabulaire officiel défini dans [docs/GLOSSARY.md](../docs/GLOSSARY.md),
- être compréhensible sans connaissance implicite du contexte de son auteur,
- être écrit intégralement en anglais pour les éléments techniques (code, fichiers, modules), le français restant réservé à la documentation métier et aux échanges.

## 2. Fichiers

- Nom en `kebab-case` (minuscules, séparées par des tirets), ex. `import-engine.md`, `forecast-service.md`.
- Le nom du fichier reflète son contenu principal, sans abréviation non explicite.
- Les fichiers de documentation de référence (comme ceux de `docs/`, `branding/`, `architecture/`) utilisent `UPPER_SNAKE_CASE.md`, conformément à la convention déjà établie dans ces dossiers.

## 3. Classes (et entités de Domaine)

- `PascalCase`, nom singulier, correspondant directement à un terme du [Glossaire](../docs/GLOSSARY.md) : `Organisation`, `Commercial`, `Facture`, `Prevision`.
- Une classe technique de support (non métier) suit également le `PascalCase` mais explicite son rôle : `ImportValidator`, `ForecastCalculator`.
- Aucune classe ne doit porter un nom générique non descriptif (`Manager`, `Handler`, `Data` seuls, sans qualification).

## 4. Fonctions (et méthodes)

- `camelCase`, verbe d'action explicite en préfixe : `calculateForecast`, `validateImport`, `generateReport`.
- Une fonction qui retourne un booléen est préfixée par `is`, `has` ou `can` : `isLicenseValid`, `hasAccess`.
- Le nom d'une fonction doit décrire ce qu'elle fait, jamais comment elle le fait.

## 5. Variables

- `camelCase`, nom explicite et non abrégé : `salesTarget`, `currentPeriod`, `organizationId`.
- Aucune variable à lettre unique en dehors des indices de boucle strictement locaux et de portée réduite.
- Les booléens sont nommés selon le même principe que les fonctions booléennes (`isActive`, `hasPermission`).

## 6. Constantes

- `UPPER_SNAKE_CASE` : `MAX_IMPORT_SIZE`, `DEFAULT_FORECAST_HORIZON`.
- Toute valeur métier significative (seuil, limite, durée) doit être nommée en constante, jamais laissée en valeur brute dans la logique.

## 7. Modules

- `kebab-case`, nom correspondant exactement à l'appellation officielle définie dans [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) : `import-engine`, `forecast-engine`, `plugin-system`.
- Le nom d'un module ne change jamais après sa première publication, afin de préserver la stabilité des références et intégrations externes (cf. [VERSIONING_STRATEGY.md](VERSIONING_STRATEGY.md)).

## 8. Packages (regroupements internes à un module)

- `kebab-case` ou équivalent en minuscules selon la convention du langage retenu, reflétant la couche applicative concernée : `domain`, `application`, `infrastructure`, `presentation` (cf. [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md)).
- Un package ne porte jamais le nom d'une technologie (`utils`, `helpers`, `common` sont proscrits comme noms de package fourre-tout) ; il porte le nom du concept qu'il regroupe.

## 9. Branches Git

- Format : `<type>/<court-descriptif-en-kebab-case>`, ex. `feature/forecast-horizon-config`, `fix/import-duplicate-detection`, `docs/naming-conventions`.
- Types autorisés : `feature`, `fix`, `docs`, `refactor`, `chore`.
- Le descriptif reste bref (5 mots maximum) et compréhensible sans contexte additionnel.

## 10. Commits

- Format : `<type>: <résumé au présent, en minuscule, sans point final>`, ex. `feat: add forecast horizon configuration`, `fix: prevent duplicate import detection failure`.
- Types alignés sur ceux des branches (`feat`, `fix`, `docs`, `refactor`, `chore`).
- Le résumé décrit l'effet du changement, jamais le détail technique de sa réalisation (cf. principe "pourquoi plutôt que quoi", cohérent avec [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md)).
- Un commit correspond à un changement cohérent et unique ; un commit ne mélange jamais plusieurs préoccupations non liées.

## 11. Cohérence transversale

Toute incohérence de nommage détectée doit être corrigée dès sa découverte plutôt que reproduite par cohérence apparente avec l'erreur existante : la présente convention prévaut toujours sur l'usage déjà en place qui s'en écarterait.
