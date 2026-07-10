# ORBITA AI — Design Principles (Design System)

Ce document définit les règles structurelles auxquelles toute future interface ORBITA AI doit se conformer. Il ne contient ni code ni maquette : uniquement les règles de composition.

## 1. Grille

- Grille de base : **8px** (toute dimension — espacement, taille de composant — est un multiple de 8, avec un demi-pas de 4px toléré pour les ajustements fins de texte).
- Grille de mise en page : **12 colonnes**, gouttière de 24px, marges extérieures de 32px (desktop) / 16px (mobile).
- Largeur de contenu maximale recommandée pour la lecture de texte : 720px ; les tableaux et graphiques peuvent occuper toute la largeur disponible.

## 2. Espacements (spacing scale)

| Token | Valeur |
|---|---|
| `space.xs` | 4px |
| `space.sm` | 8px |
| `space.md` | 16px |
| `space.lg` | 24px |
| `space.xl` | 32px |
| `space.2xl` | 48px |
| `space.3xl` | 64px |

Règle : l'espacement entre deux éléments de même niveau hiérarchique doit toujours être inférieur à l'espacement qui sépare deux groupes différents (principe de proximité).

## 3. Rayons (border-radius)

| Token | Valeur | Usage |
|---|---|---|
| `radius.sm` | 4px | Champs, petits boutons, badges |
| `radius.md` | 8px | Boutons standards, inputs |
| `radius.lg` | 12px | Cartes, panneaux |
| `radius.xl` | 16px | Modales, fenêtres flottantes |
| `radius.full` | 999px | Pastilles, avatars, indicateurs d'état |

Les rayons restent modérés (jamais > 16px hors pastilles) pour préserver un caractère technique et précis plutôt que "doux/ludique".

## 4. Ombres (elevation)

Sur fond sombre, l'élévation se traduit avant tout par un changement de teinte de surface (cf. [COLOR_SYSTEM.md](COLOR_SYSTEM.md) §7) complété d'une ombre discrète :

| Niveau | Token | Valeur (référence) |
|---|---|---|
| Niveau 0 | `shadow.none` | aucune |
| Niveau 1 (carte) | `shadow.sm` | 0 1px 2px rgba(0,0,0,0.32) |
| Niveau 2 (carte survolée / menu) | `shadow.md` | 0 4px 12px rgba(0,0,0,0.40) |
| Niveau 3 (modale, popover) | `shadow.lg` | 0 12px 32px rgba(0,0,0,0.48) |

Aucune ombre colorée ou "glow" néon n'est utilisée par défaut ; un halo discret en `brand.primary` à faible opacité (≤ 12%) est toléré uniquement pour signaler un état actif de mise en avant (ex. carte sélectionnée), jamais de façon systématique.

## 5. Animations et transitions

Voir le détail complet dans [MOTION_GUIDELINES.md](MOTION_GUIDELINES.md). Règle de synthèse : toute transition d'interface reste courte (150–250ms), avec une courbe de type "ease-out", jamais de rebond (bounce) ni d'élasticité.

## 6. Cartes (cards)

- Fond : `surface.default`, bordure `border.subtle` 1px.
- Rayon : `radius.lg`.
- Padding interne : `space.lg` (24px) minimum.
- Hiérarchie interne : titre (`H4`), contenu (`body`/`body-sm`), métadonnées (`caption`) alignées en pied de carte.
- Une carte ne contient jamais plus d'une action primaire.

## 7. Tableaux

- Ligne d'en-tête fixe, fond `bg.raised`, texte `text.secondary`, `caption` en majuscule tracking large (style `overline`).
- Hauteur de ligne standard : 44px (dense : 36px, pour les vues à fort volume de données).
- Alternance de fond proscrite (zebra striping) : la séparation se fait uniquement par une bordure `border.subtle` fine entre lignes, pour un rendu plus sobre.
- Les colonnes de valeurs numériques sont alignées à droite avec chiffres tabulaires (cf. [TYPOGRAPHY.md](TYPOGRAPHY.md)).
- Ligne survolée : fond `surface.raised`.
- Ligne sélectionnée : bordure gauche 2px `brand.primary`.

## 8. Graphiques (charts)

- Fond transparent, s'intégrant à la surface qui l'accueille.
- Grille de fond (gridlines) : `border.subtle`, jamais plus foncée que nécessaire.
- Axes et labels : `text.secondary`, `caption`.
- Séries : palette définie dans [COLOR_SYSTEM.md](COLOR_SYSTEM.md) §10, dans l'ordre indiqué.
- Courbes à trait plein 2px ; aucun effet 3D, aucun dégradé de remplissage saturé (un dégradé vertical discret vers la transparence est toléré sous une courbe unique pour signaler une aire, jamais sous plusieurs séries superposées).

## 9. Formulaires

- Champ standard : hauteur 40px, `radius.md`, fond `surface.input`, bordure `border.default`.
- État focus : bordure `brand.primary` 1.5px + anneau de focus (cf. [ACCESSIBILITY.md](ACCESSIBILITY.md)).
- État erreur : bordure `semantic.critical`, message d'aide en `semantic.critical`, `caption`.
- Libellés (`label`) toujours au-dessus du champ, jamais uniquement en placeholder.

## 10. Fenêtres et modales

- Largeur standard : 480px (confirmation), 640px (formulaire), 960px (contenu riche / comparatif).
- Fond `bg.overlay`, rayon `radius.xl`, ombre `shadow.lg`.
- Un scrim (voile) `rgba(6,7,11,0.72)` recouvre le contenu sous-jacent.
- Une modale contient toujours un titre, un corps, et une zone d'actions clairement séparée (bordure supérieure `border.subtle`).

## 11. Menus

- Fond `bg.overlay`, rayon `radius.md`, ombre `shadow.md`.
- Item de menu : hauteur 36px, padding horizontal `space.md`.
- Item survolé : fond `surface.raised`.
- Séparateurs : `border.subtle` 1px, marge verticale `space.xs`.

## 12. Sidebar (navigation latérale)

- Largeur standard : 264px (état déployé), 72px (état réduit, icônes seules).
- Fond : `bg.raised`, légèrement distinct du fond de contenu (`bg.base`) pour ancrer la navigation.
- Item actif : fond `surface.raised`, texte `text.primary`, indicateur vertical gauche 2px `brand.primary`.
- Regroupement des items par section, séparateur `border.subtle`, libellé de section en style `overline`.

## 13. Topbar (barre supérieure)

- Hauteur standard : 64px.
- Fond : `bg.raised`, bordure inférieure `border.subtle` 1px.
- Contenu : identité de page à gauche (titre `H3`), actions contextuelles et statut ORION à droite.
- La topbar reste fixe (sticky) lors du défilement du contenu.

## 14. Principe transversal

Tout nouveau composant doit être validable au regard d'une question simple : *"Cet élément aide-t-il directement la décision de l'utilisateur ?"* Si la réponse est non, l'élément est superflu et doit être retiré ou simplifié.
