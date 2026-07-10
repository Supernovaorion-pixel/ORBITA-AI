# ORBITA AI — Color System

Toutes les couleurs sont définies en HEX. Le système est pensé **dark-first** (l'interface produit est conçue nativement pour un fond sombre, cohérent avec le positionnement "cyberpunk premium"), avec un mode clair complet pour les contextes qui l'exigent (documents, présentations, préférence utilisateur).

## 1. Couleurs principales (Brand Primary)

| Rôle | Token | HEX |
|---|---|---|
| Primaire | `brand.primary` | `#4C6FFF` |
| Primaire — hover | `brand.primary.hover` | `#3D5CE0` |
| Primaire — active/pressed | `brand.primary.active` | `#2F49B8` |
| Primaire — tint (fond léger, mode clair) | `brand.primary.tint` | `#EAF0FF` |

La couleur primaire (bleu orbital) est la signature de la marque : CTA principaux, liens, éléments actifs, focus de marque.

## 2. Couleurs secondaires

| Rôle | Token | HEX |
|---|---|---|
| Secondaire — Violet orbital | `brand.secondary` | `#9D7BFF` |
| Secondaire — hover | `brand.secondary.hover` | `#8863F0` |
| Secondaire — tint | `brand.secondary.tint` | `#F1ECFF` |

Le violet est utilisé en soutien du bleu primaire (accents secondaires, éléments IA/ORION, dégradés de marque très discrets et toujours à deux tons maximum).

## 3. Couleur d'accent

| Rôle | Token | HEX |
|---|---|---|
| Accent — Cyan orbital | `accent.cyan` | `#22D3EE` |
| Accent — hover | `accent.cyan.hover` | `#0FB8D4` |

Le cyan est réservé aux éléments de mise en avant ponctuels : indicateurs "live", curseurs de graphique actifs, éléments d'interaction propres à ORION.

## 4. Couleurs sémantiques

| Rôle | Token | HEX |
|---|---|---|
| Succès | `semantic.success` | `#1FD68F` |
| Succès — fond | `semantic.success.bg` | `#0E2B23` (dark) / `#E6FBF3` (light) |
| Information | `semantic.info` | `#3AA0FF` |
| Information — fond | `semantic.info.bg` | `#0D2540` (dark) / `#E8F3FF` (light) |
| Avertissement | `semantic.warning` | `#FFB020` |
| Avertissement — fond | `semantic.warning.bg` | `#332305` (dark) / `#FFF6E0` (light) |
| Critique / Erreur | `semantic.critical` | `#FF4D6A` |
| Critique — fond | `semantic.critical.bg` | `#3A0F19` (dark) / `#FFE9ED` (light) |

## 5. Fonds (Backgrounds) — mode sombre (référence produit)

| Rôle | Token | HEX |
|---|---|---|
| Fond de base | `bg.base` | `#06070B` |
| Fond élevé (niveau 1) | `bg.raised` | `#0B0D14` |
| Fond élevé (niveau 2 — modales, popovers) | `bg.overlay` | `#12141D` |

## 6. Fonds — mode clair

| Rôle | Token | HEX |
|---|---|---|
| Fond de base | `bg.base.light` | `#F7F8FB` |
| Fond élevé | `bg.raised.light` | `#FFFFFF` |
| Fond overlay | `bg.overlay.light` | `#FFFFFF` |

## 7. Surfaces

| Rôle | Token | HEX (dark) | HEX (light) |
|---|---|---|---|
| Surface standard (cartes) | `surface.default` | `#10131C` | `#FFFFFF` |
| Surface élevée (cartes actives/hover) | `surface.raised` | `#161A25` | `#F9FAFC` |
| Surface d'entrée (inputs) | `surface.input` | `#0D0F17` | `#FFFFFF` |

## 8. Bordures

| Rôle | Token | HEX (dark) | HEX (light) |
|---|---|---|---|
| Bordure discrète | `border.subtle` | `#1E2230` | `#E7E9F0` |
| Bordure standard | `border.default` | `#232838` | `#DDE1EA` |
| Bordure forte / focus non-marque | `border.strong` | `#323952` | `#C7CCDA` |

## 9. Textes

| Rôle | Token | HEX (dark) | HEX (light) |
|---|---|---|---|
| Texte primaire | `text.primary` | `#F5F7FA` | `#0B0D14` |
| Texte secondaire | `text.secondary` | `#A6ADC2` | `#4A5170` |
| Texte tertiaire / placeholder | `text.tertiary` | `#6B7290` | `#7A81A0` |
| Texte désactivé | `text.disabled` | `#454B63` | `#B4B9C8` |
| Texte inversé (sur fond de couleur) | `text.inverse` | `#06070B` | `#FFFFFF` |

Ratios de contraste minimums : voir [ACCESSIBILITY.md](ACCESSIBILITY.md).

## 10. Palette graphique (data visualization)

Utilisée pour les graphiques, séries de données et indicateurs analytiques. Ordre d'utilisation prioritaire de gauche à droite :

| Ordre | Nom | HEX |
|---|---|---|
| 1 | Bleu orbital | `#4C6FFF` |
| 2 | Cyan orbital | `#22D3EE` |
| 3 | Violet orbital | `#9D7BFF` |
| 4 | Vert signal | `#1FD68F` |
| 5 | Ambre | `#FFB020` |
| 6 | Magenta signal | `#FF4D6A` |
| 7 | Slate (neutre graphique) | `#7A88B8` |
| 8 | Rose orbital | `#D66BFF` |

Règle : ne jamais dépasser 8 séries simultanées sur un même graphique ; au-delà, regrouper sous "Autres" (`text.secondary`).

## 11. États des boutons

### Bouton primaire
| État | Fond | Texte | Bordure |
|---|---|---|---|
| Défaut | `#4C6FFF` | `#FFFFFF` | — |
| Hover | `#3D5CE0` | `#FFFFFF` | — |
| Actif / pressed | `#2F49B8` | `#FFFFFF` | — |
| Focus | `#4C6FFF` | `#FFFFFF` | anneau `#22D3EE` 2px (voir [ACCESSIBILITY.md](ACCESSIBILITY.md)) |
| Désactivé | `#232838` | `#6B7290` | — |

### Bouton secondaire (outline)
| État | Fond | Texte | Bordure |
|---|---|---|---|
| Défaut | transparent | `#F5F7FA` | `#323952` |
| Hover | `#161A25` | `#F5F7FA` | `#4C6FFF` |
| Actif | `#0D0F17` | `#F5F7FA` | `#3D5CE0` |
| Désactivé | transparent | `#454B63` | `#1E2230` |

### Bouton destructif
| État | Fond | Texte |
|---|---|---|
| Défaut | `#FF4D6A` | `#FFFFFF` |
| Hover | `#E6415C` | `#FFFFFF` |
| Actif | `#C1334A` | `#FFFFFF` |
| Désactivé | `#3A0F19` | `#6B7290` |

## 12. Règles d'usage

- La couleur ne sert jamais de décoration : elle encode toujours un état, une priorité ou une action.
- Le fond sombre (`bg.base` = `#06070B`) est le contexte de référence du produit ; le mode clair est une déclinaison complète, jamais une simple inversion automatique non vérifiée.
- Les couleurs sémantiques (succès, info, avertissement, critique) ne doivent jamais être substituées les unes aux autres ni réutilisées à des fins purement décoratives.
- Les dégradés sont tolérés uniquement entre deux couleurs adjacentes de la marque (ex. `#4C6FFF` → `#9D7BFF`, ou `#4C6FFF` → `#22D3EE`), jamais entre plus de deux teintes, jamais de type "arc-en-ciel".
