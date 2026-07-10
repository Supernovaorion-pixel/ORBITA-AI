# ORBITA AI — Typography

## 1. Principes

La typographie d'ORBITA AI doit exprimer la précision et la rigueur analytique. Elle privilégie des caractères géométriques ou néo-grotesques, à haute lisibilité sur écran, disponibles en de nombreuses graisses pour construire une hiérarchie fine.

Aucune police décorative, manuscrite ou "script" n'est autorisée, en aucune circonstance.

## 2. Police principale

**Famille recommandée : Inter** (ou toute police néo-grotesque équivalente de qualité comparable — ex. Suisse Int'l, General Sans — à titre de repli si une licence spécifique est requise).

Usage : ensemble de l'interface produit, tableaux de bord, formulaires, texte courant, tableaux de données.

Raisons du choix :
- excellente lisibilité aux petites tailles (données chiffrées, tableaux denses),
- chiffres tabulaires disponibles (alignement des colonnes numériques),
- large palette de graisses (300 à 800),
- caractère neutre et technique, cohérent avec le positionnement SaaS haut de gamme.

## 3. Police secondaire (technique / monospace)

**Famille recommandée : JetBrains Mono** (ou Roboto Mono en repli).

Usage strictement réservé à :
- valeurs de code, identifiants techniques, tokens système,
- horodatages et identifiants de logs,
- métriques nécessitant un alignement caractère par caractère (ex. adresses, hash, référence de rapport).

Ne jamais utiliser la police secondaire pour du texte courant ou des titres.

## 4. Police d'affichage de marque (optionnelle, communication uniquement)

Pour les supports de marque hors produit (site vitrine, présentations investisseurs), une variante plus affirmée du même caractère (graisses 600–800 d'Inter) peut être utilisée pour les grands titres, afin de conserver une seule famille typographique dans tout l'écosystème de marque.

## 5. Échelle typographique

| Niveau | Token | Taille | Interlignage | Graisse | Usage |
|---|---|---|---|---|---|
| Display | `type.display` | 40px | 48px | 700 | Écrans d'accueil, pages marketing |
| H1 | `type.h1` | 32px | 40px | 700 | Titre de page |
| H2 | `type.h2` | 24px | 32px | 600 | Titre de section |
| H3 | `type.h3` | 20px | 28px | 600 | Titre de bloc / carte |
| H4 | `type.h4` | 16px | 24px | 600 | Sous-titre, en-tête de tableau |
| Body Large | `type.body-lg` | 16px | 24px | 400 | Texte courant principal |
| Body | `type.body` | 14px | 20px | 400 | Texte courant standard (défaut UI) |
| Body Small | `type.body-sm` | 13px | 18px | 400 | Légendes, texte secondaire |
| Caption | `type.caption` | 12px | 16px | 500 | Étiquettes, métadonnées, badges |
| Overline | `type.overline` | 11px | 14px | 600 | Éyettes de section, majuscules, tracking large |
| Data / Mono | `type.data` | 13px | 18px | 500 | Valeurs chiffrées, code, IDs |

## 6. Hiérarchie

1. Une seule taille `Display` ou `H1` par écran.
2. Ne jamais sauter plus d'un niveau hiérarchique consécutif dans une même vue (ex. ne pas passer directement de H1 à H4).
3. Le poids (graisse) porte autant la hiérarchie que la taille : préférer une variation de graisse à une multiplication de tailles.
4. Les valeurs chiffrées critiques (KPI, montants) utilisent systématiquement des chiffres tabulaires (`font-variant-numeric: tabular-nums`) pour garantir l'alignement dans les tableaux et cartes de métriques.

## 7. Graisses (weights)

| Graisse | Valeur | Usage |
|---|---|---|
| Regular | 400 | Texte courant |
| Medium | 500 | Emphase légère, captions, données |
| Semibold | 600 | Titres de niveau H2 à H4, boutons |
| Bold | 700 | Titres H1/Display, valeurs KPI mises en avant |

Les graisses inférieures à 400 (Light, Thin) sont proscrites : elles nuisent à la lisibilité sur fond sombre à petite taille.

## 8. Espacement (letter-spacing & tracking)

| Niveau | Tracking |
|---|---|
| Display / H1 / H2 | -0.02em (resserré, pour un rendu premium) |
| H3 / H4 / Body | 0 (normal) |
| Caption | 0 |
| Overline | +0.08em (majuscules, aéré) |

## 9. Interdictions

- Ne jamais mélanger plus de deux familles typographiques dans un même écran (principale + monospace).
- Ne jamais utiliser de police avec empattements (serif) dans le produit.
- Ne jamais utiliser de texte tout en majuscules en dehors des `overline` et badges d'état courts.
- Ne jamais descendre en dessous de 12px pour un texte porteur d'information (voir tailles minimales dans [ACCESSIBILITY.md](ACCESSIBILITY.md)).
