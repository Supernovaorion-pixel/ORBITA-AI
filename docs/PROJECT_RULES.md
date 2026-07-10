# ORBITA AI — Project Rules

Ce document fixe les règles officielles et permanentes du projet ORBITA AI. Elles s'appliquent à toute personne contribuant au projet, quel que soit son rôle, et à toute future étape de développement.

## 1. Une seule source de vérité

Toute information de référence (vision, spécifications, vocabulaire, identité de marque) existe en un seul endroit officiel. En cas de doute ou de contradiction apparente, le document de référence correspondant (ce dossier `docs/` et le dossier [`branding/`](../branding/)) prévaut sur toute autre source (mémoire individuelle, échange informel, ancienne version).

## 2. Aucune duplication

Une même information ne doit jamais être redéfinie à deux endroits différents. Toute nouvelle documentation doit faire référence au document existant plutôt que d'en recopier ou reformuler le contenu.

## 3. Documentation obligatoire

Aucune fonctionnalité, aucun module, aucune décision structurante ne peut être considéré comme validé s'il n'est pas documenté. L'absence de documentation équivaut à l'absence de décision.

## 4. Documentation avant développement

Toute nouvelle fonctionnalité doit être spécifiée dans la documentation appropriée avant le début de son développement. La documentation précède et encadre le développement, elle ne le suit pas.

## 5. Code lisible

Lorsque le développement débutera, le code devra toujours privilégier la clarté à la concision. Un code doit pouvoir être compris par un futur contributeur sans connaissance préalable des intentions de son auteur (cf. principe de lisibilité, [TECHNICAL_SPECIFICATION.md](TECHNICAL_SPECIFICATION.md)).

## 6. Architecture modulaire

Le logiciel est structuré en modules aux responsabilités clairement délimitées, conformément à [FUNCTIONAL_SPECIFICATION.md](FUNCTIONAL_SPECIFICATION.md) et [TECHNICAL_SPECIFICATION.md](TECHNICAL_SPECIFICATION.md). Aucune fonctionnalité ne doit être développée en dehors de cette structure ou en mélangeant les responsabilités de plusieurs modules.

## 7. Nommage cohérent

Tout terme utilisé dans le projet — documentation, interface, échanges internes — doit respecter le vocabulaire officiel défini dans [GLOSSARY.md](GLOSSARY.md). Aucun synonyme informel ne doit se substituer à un terme officiel une fois celui-ci défini.

## 8. Compatibilité ascendante

Toute évolution du logiciel doit préserver, sauf décision explicite et documentée, la compatibilité avec les usages, données et configurations déjà en place chez les organisations clientes. Une rupture de compatibilité ne peut être qu'un choix assumé, jamais un effet de bord.

## 9. Tests avant fusion

Lorsque le développement débutera, aucune évolution du logiciel ne pourra être intégrée à la version de référence sans avoir été préalablement vérifiée. La vérification précède l'intégration, jamais l'inverse.

## 10. Séparation des responsabilités du projet

- La documentation fondatrice du projet (ce dossier `docs/`) et l'identité de marque ([`branding/`](../branding/)) sont des domaines distincts, chacun disposant de ses propres règles et responsables.
- Aucune décision de branding ne doit être prise dans la documentation fonctionnelle ou technique, et réciproquement.

## 11. Exigence de pérennité

Toute décision — documentaire, fonctionnelle ou technique — doit être prise en considérant sa validité à plusieurs années, conformément à la vision du projet ([PROJECT_VISION.md](PROJECT_VISION.md)). Une solution de facilité qui compromettrait cette pérennité n'est pas acceptable.

## 12. Primauté de l'utilisateur final

Toute règle, tout principe technique et toute décision de conception restent subordonnés à un objectif final : permettre à l'utilisateur d'ORBITA AI de décider plus vite et avec plus de confiance (cf. [PROJECT_VISION.md](PROJECT_VISION.md) et [USER_PERSONAS.md](USER_PERSONAS.md)). En cas de conflit entre une règle et l'intérêt réel de l'utilisateur, la règle doit être réexaminée, jamais imposée aveuglément.
