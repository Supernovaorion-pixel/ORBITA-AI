# ORBITA AI — Motion Guidelines

## 1. Principe directeur

Le mouvement dans ORBITA AI sert exclusivement à **clarifier**, jamais à **impressionner**. Chaque animation doit renforcer la compréhension d'un changement d'état (apparition, transition, chargement) et non ajouter une dimension décorative.

L'inspiration orbitale peut se traduire par des mouvements fluides, continus et circulaires dans des contextes très spécifiques (chargement, statut actif d'ORION), mais jamais de façon généralisée à toute l'interface.

## 2. Durées

| Type d'interaction | Durée | Usage |
|---|---|---|
| Micro-interaction | 100–150ms | Survol de bouton, changement de couleur d'icône |
| Transition standard | 150–250ms | Ouverture de menu, apparition de tooltip, changement d'onglet |
| Transition de composant | 250–350ms | Ouverture/fermeture de modale, panneau latéral |
| Transition de page | 300–400ms | Changement de vue complète |
| Animation de statut ORION | 800–1500ms (boucle) | Indicateur de traitement/analyse en cours |

Aucune animation d'interface standard ne doit dépasser 400ms : au-delà, elle est perçue comme un ralentissement plutôt qu'un raffinement.

## 3. Courbes d'accélération (easing)

| Token | Courbe | Usage |
|---|---|---|
| `ease.standard` | cubic-bezier(0.4, 0, 0.2, 1) | Transitions générales (ease-in-out) |
| `ease.out` | cubic-bezier(0, 0, 0.2, 1) | Éléments qui apparaissent (ease-out) |
| `ease.in` | cubic-bezier(0.4, 0, 1, 1) | Éléments qui disparaissent (ease-in) |
| `ease.orbital` | cubic-bezier(0.45, 0, 0.55, 1) | Animations circulaires/continues propres à ORION |

Aucune courbe à rebond (bounce, elastic, spring exagéré) n'est autorisée : ce type de mouvement évoque un registre ludique incompatible avec le positionnement premium.

## 4. Types de transitions autorisées

- **Fondu (fade)** : apparition/disparition d'éléments non spatialement liés à une action (tooltips, toasts).
- **Glissement (slide)** : panneaux latéraux, tiroirs (drawers), menus contextuels — glissement court, 8 à 16px de déplacement maximum combiné à un fondu.
- **Échelle (scale)** subtile (98% → 100%) : ouverture de modales et popovers, jamais au-delà de ±4% pour rester discret.
- **Rotation continue** : réservée exclusivement à l'indicateur de statut/chargement d'ORION (anneau orbital tournant), jamais utilisée comme transition de contenu.

## 5. Micro-interactions

- Bouton au survol : changement de couleur de fond uniquement (voir [COLOR_SYSTEM.md](COLOR_SYSTEM.md) §11), sans changement de taille.
- Bouton au clic : léger assombrissement (état `active`), sans effet de compression/rebond.
- Champ de formulaire au focus : apparition de l'anneau de focus en 100ms, sans délai perceptible.
- Ligne de tableau au survol : changement de fond instantané ou en 100ms maximum.

## 6. Indicateur de statut ORION

- L'état "ORION analyse" est représenté par une animation orbitale sobre : un ou deux points lumineux discrets parcourant une trajectoire circulaire ou elliptique fine autour du symbole, à vitesse constante (pas d'accélération/décélération), en boucle continue.
- Couleur de l'animation : `accent.cyan` (`#22D3EE`) ou `brand.primary` (`#4C6FFF`) à opacité modérée.
- L'animation cesse immédiatement et sans transition de sortie superflue dès que le traitement est terminé (retour à l'état statique).

## 7. Chargement et états transitoires

- Les états de chargement utilisent un indicateur linéaire fin (barre de progression) ou l'indicateur orbital (§6), jamais de spinner générique décoratif emprunté à d'autres registres visuels.
- Les squelettes de chargement (skeleton screens) sont autorisés pour les tableaux et cartes de données, avec un effet de pulsation douce (opacité 60–100%, 1.2s, `ease.orbital`).

## 8. Interdictions

- Aucune animation à but purement décoratif (particules, effets de fond animés en continu, parallax excessif).
- Aucun effet "glitch", scan-line, ou distorsion numérique — ces effets relèvent d'un registre cyberpunk grand public/gaming explicitement exclu du positionnement (cf. [BRAND_GUIDELINES.md](BRAND_GUIDELINES.md)).
- Aucune animation ne doit bloquer l'accès à une action pendant sa durée (l'animation accompagne l'état, elle ne le retarde pas).
- Respecter systématiquement la préférence système "réduire les animations" (`prefers-reduced-motion`) : dans ce cas, toutes les transitions non essentielles sont remplacées par un changement d'état instantané, et l'indicateur orbital d'ORION passe en variante statique (icône fixe + pulsation d'opacité uniquement).
