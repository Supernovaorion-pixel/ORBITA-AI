# ORBITA AI — UX Principles

> Ce document définit la philosophie d'expérience utilisateur officielle d'ORBITA AI. Il constitue la référence absolue de toute future interface : aucun écran, aucun composant futur ne doit s'en écarter. Il ne contient ni code, ni maquette, ni choix technique.

## 1. Univers de conception

L'expérience ORBITA AI s'inscrit dans un univers **Neo Futurism Enterprise / Cyberpunk Premium** : une technologie de pointe au service de la décision, jamais un décor. Cet univers hérite directement de l'identité définie dans [branding/BRAND_GUIDELINES.md](../branding/BRAND_GUIDELINES.md) et se traduit dans l'UX par quatre qualités constantes :

- **Minimaliste** — chaque écran ne présente que ce qui sert la décision en cours.
- **Professionnel** — sobriété, précision, absence de toute familiarité gratuite.
- **Dark-first** — l'interface est conçue nativement pour un fond sombre (cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md)), le mode clair en étant une déclinaison complète, jamais l'inverse.
- **Technologique** — la mise en page, les transitions et les données donnent une impression immédiate de précision et de maîtrise.

## 2. Ce que l'expérience doit inspirer

Toute interface ORBITA AI doit, dès les premières secondes de consultation, inspirer :

| Sensation | Traduction en conception |
|---|---|
| **Performance** | Réactivité perçue immédiate, absence de friction inutile (cf. [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md)). |
| **Précision** | Hiérarchie visuelle stricte, données alignées, jamais d'approximation graphique. |
| **Innovation** | Un vocabulaire visuel avancé mais jamais gadget — la nouveauté sert la clarté, jamais l'esthétique seule. |
| **Confiance** | Cohérence absolue d'un écran à l'autre ; rien qui ne surprenne l'utilisateur négativement. |
| **Rapidité** | Un chemin court entre l'ouverture d'un écran et la réponse à la question que l'utilisateur se pose. |

## 3. Ce qui est explicitement exclu

- **Aucune inspiration gaming** : pas de HUD, pas de barres de vie, pas d'effets de score, pas de vocabulaire ludique.
- **Aucun effet tape-à-l'œil** : pas de néon saturé, pas de glitch, pas d'animation décorative sans fonction (cf. [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) §8).
- **Aucune surcharge visuelle** : la densité d'information est toujours arbitrée en faveur de la lisibilité, jamais de l'exhaustivité à tout prix.

## 4. Principes directeurs transversaux

### 4.1 Une question, une réponse
Chaque écran est conçu pour répondre à une question précise que se pose un des profils définis dans [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md). Un écran qui répond à plusieurs questions sans hiérarchie claire doit être reconsidéré.

### 4.2 Priorité à la décision, jamais à la donnée brute
La donnée n'est jamais affichée pour elle-même : elle est toujours présentée dans son contexte de décision (objectif, écart, tendance). Cf. [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md).

### 4.3 Cohérence absolue
Un composant, un comportement ou un motif visuel a un seul sens dans tout le produit. Un même signal (couleur, icône, position) ne doit jamais signifier deux choses différentes selon l'écran.

### 4.4 Densité maîtrisée
L'interface privilégie l'espace et la respiration (cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §2) à la compression d'information. Une vue dense (tableau, historique) reste organisée et jamais confuse.

### 4.5 Progressivité de la complexité
Un écran présente d'abord la synthèse, puis permet d'accéder au détail sur demande explicite de l'utilisateur (cf. [USER_FLOWS.md](USER_FLOWS.md)). L'utilisateur ne doit jamais être submergé par défaut.

### 4.6 ORION comme présence constante, jamais intrusive
ORION est accessible depuis tout écran, sans jamais s'imposer à l'utilisateur ni interrompre son parcours (cf. section ORION de [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md)).

## 5. Rôle de ce document

[DESIGN_SYSTEM.md](DESIGN_SYSTEM.md), [NAVIGATION.md](NAVIGATION.md), [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md) et l'ensemble des autres documents de ce dossier déclinent ces principes en règles opérationnelles. En cas de doute ou d'ambiguïté sur une règle plus spécifique, ce document prévaut.
