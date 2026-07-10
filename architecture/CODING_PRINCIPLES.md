# ORBITA AI — Coding Principles

> Ce document définit les principes de conception du code que toute future implémentation devra respecter. Aucun langage, framework ou syntaxe n'est prescrit : ces principes sont universels et doivent guider chaque décision de code, quel que soit l'environnement technique retenu.

## 1. SOLID

- **Responsabilité unique (Single Responsibility)** : une classe, une fonction ou un module n'a qu'une seule raison de changer. Cf. §7 et [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md), où chaque module est défini par une responsabilité unique.
- **Ouvert/fermé (Open/Closed)** : le code doit être ouvert à l'extension (ex. un nouveau connecteur, un nouveau moteur d'IA via [EXTENSIBILITY.md](EXTENSIBILITY.md)) sans nécessiter la modification du code existant et validé.
- **Substitution de Liskov** : toute implémentation alternative d'une interface (ex. un connecteur ERP différent) doit pouvoir se substituer à une autre sans changer le comportement attendu par les modules qui l'utilisent.
- **Ségrégation des interfaces** : une interface expose uniquement ce dont ses utilisateurs ont réellement besoin ; aucune interface ne doit forcer un module à dépendre de capacités qu'il n'utilise pas.
- **Inversion des dépendances** : les couches stables (Domaine, Application) définissent des interfaces ; les couches périphériques (Infrastructure) les implémentent. Le Domaine ne dépend jamais de l'Infrastructure (cf. [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md) §3).

## 2. KISS (Keep It Simple)

- La solution la plus simple qui répond correctement au besoin est toujours préférée à une solution plus sophistiquée mais non requise.
- La complexité n'est introduite que lorsqu'un besoin réel et documenté l'exige, jamais par anticipation ou par goût technique.
- Un code simple à lire est un code qui a moins de chances de contenir une erreur non détectée, et qui reste maintenable sur dix ans.

## 3. DRY (Don't Repeat Yourself)

- Une règle métier, une logique de calcul ou une constante n'est définie qu'à un seul endroit du système.
- Une duplication constatée entre deux modules doit être résolue par l'extraction d'une capacité partagée au niveau approprié (Core ou Services), jamais tolérée par commodité.
- La duplication de code est acceptable uniquement lorsque les deux occurrences répondent à des besoins métier réellement distincts qui évolueront différemment dans le temps — la duplication par simplicité de copier-coller ne l'est jamais.

## 4. YAGNI (You Aren't Gonna Need It)

- Aucune fonctionnalité, paramètre de configuration ou couche d'abstraction n'est développée en prévision d'un besoin hypothétique non exprimé.
- L'extensibilité du système (cf. [EXTENSIBILITY.md](EXTENSIBILITY.md)) est assurée par la qualité de son architecture modulaire, pas par l'anticipation spéculative de chaque cas futur possible.
- Une capacité non utilisée après son introduction doit être retirée plutôt que maintenue "au cas où".

## 5. Responsabilité unique appliquée au code

Au-delà du niveau module, ce principe s'applique à chaque classe et fonction : une fonction qui importe une donnée ne doit pas également l'analyser ; une classe qui représente une entité de Domaine ne doit pas porter la logique de sa restitution visuelle (cf. séparation des couches, [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md)).

## 6. Composition plutôt qu'héritage

- Les comportements complexes sont construits par assemblage de composants simples et indépendants, plutôt que par des hiérarchies d'héritage profondes et rigides.
- L'héritage n'est utilisé que pour exprimer une relation de spécialisation réelle et stable dans le temps, jamais comme raccourci pour partager du code.
- La composition favorise directement la modularité et l'évolutivité exigées par l'architecture globale (cf. [SYSTEM_ARCHITECTURE.md](SYSTEM_ARCHITECTURE.md)).

## 7. Lisibilité

- Le code est écrit pour être lu par un humain avant d'être exécuté par une machine.
- Le nommage suit strictement les conventions définies dans [NAMING_CONVENTIONS.md](NAMING_CONVENTIONS.md), garantissant qu'un lecteur retrouve dans le code le vocabulaire métier du [Glossaire](../docs/GLOSSARY.md).
- Une fonction ou une classe qui nécessite un effort important de lecture pour être comprise doit être simplifiée ou décomposée, jamais commentée pour compenser sa complexité.

## 8. Documentation

- Toute règle métier non évidente à la lecture du code doit être documentée à l'endroit où elle est implémentée, expliquant le "pourquoi", jamais le "quoi" (déjà exprimé par le code lui-même).
- Aucune fonctionnalité n'est développée sans documentation préalable de son comportement attendu, conformément à [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §4.
- La documentation d'architecture (ce dossier) prévaut toujours sur un commentaire de code en cas de contradiction ; un tel écart doit être corrigé dès sa découverte.

## 9. Principe d'arbitrage

Lorsque deux principes semblent entrer en tension (par exemple DRY et YAGNI, si l'extraction d'une capacité partagée semble prématurée), la décision doit être documentée explicitement, en expliquant l'arbitrage retenu et sa justification (cf. [ARCHITECTURE_DECISIONS.md](ARCHITECTURE_DECISIONS.md)), jamais tranchée de façon implicite ou laissée à l'appréciation ponctuelle d'un seul contributeur.
