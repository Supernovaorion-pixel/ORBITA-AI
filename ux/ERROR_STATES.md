# ORBITA AI — Error States

> Ce document définit la représentation des erreurs côté interface utilisateur. Il complète, du point de vue UX, les principes définis dans [architecture/ERROR_HANDLING.md](../architecture/ERROR_HANDLING.md) : ce dernier définit comment le système gère une erreur, celui-ci définit comment elle est présentée à l'utilisateur.

## 1. Principe général

Une erreur affichée à l'utilisateur est toujours **claire, factuelle et actionnable** (cf. [architecture/ERROR_HANDLING.md](../architecture/ERROR_HANDLING.md) §4). Elle ne doit jamais provoquer d'inquiétude disproportionnée par rapport à la gravité réelle de la situation, conformément à la sobriété exigée par la personnalité de marque (cf. [branding/BRAND_GUIDELINES.md](../branding/BRAND_GUIDELINES.md)).

## 2. Catégories d'erreur et représentation

### 2.1 Erreur de saisie (formulaire)
- Signalée immédiatement sous le champ concerné (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) §3), avec une bordure de couleur sémantique critique et un message précis sur la correction attendue.
- Ne bloque que le champ concerné ; les autres champs déjà valides du formulaire restent inchangés.

### 2.2 Erreur de donnée (Import)
- Présentée dans l'écran **Import** sous forme de liste détaillée des anomalies détectées, ligne par ligne si pertinent, permettant à l'utilisateur de comprendre précisément ce qui doit être corrigé à la source.
- Un import partiellement en échec distingue clairement ce qui a été intégré avec succès de ce qui a été rejeté.

### 2.3 Erreur d'accès
- Présentée de façon neutre, sans exposer de détail sur les droits d'un autre utilisateur ; le message indique que l'accès est restreint et, si pertinent, vers qui se tourner (Administrateur de l'Organisation).

### 2.4 Erreur d'indisponibilité (infrastructure, connecteur)
- Présentée avec un message factuel indiquant la nature indisponible temporairement du service concerné (ex. connecteur, source externe), sans détail technique interne, et une option de nouvelle tentative lorsque pertinente.

### 2.5 Erreur inattendue
- Message générique mais rassurant, invitant à réessayer ou à contacter le **Support**, jamais un message technique brut (code d'erreur système affiché seul sans explication).

## 3. Emplacement de l'erreur

- **Erreur locale** (un champ, une carte, un graphique) : affichée au plus près de l'élément concerné, sans affecter le reste de l'écran.
- **Erreur globale** (écran entier inaccessible) : remplace le contenu de la zone de contenu uniquement, la Sidebar et la Topbar restant fonctionnelles pour permettre à l'utilisateur de naviguer ailleurs.
- Une erreur n'immobilise jamais l'intégralité de l'application : la navigation reste toujours possible vers un autre écran non affecté.

## 4. Ton du message d'erreur

- Toujours à la voix factuelle, sans détourner la responsabilité ("une erreur empêche l'affichage de cette donnée" plutôt qu'une formulation vague ou dramatisée).
- Jamais d'humour, d'excuse excessive ou de familiarité, conformément au ton défini dans [branding/BRAND_GUIDELINES.md](../branding/BRAND_GUIDELINES.md) §5.

## 5. Action de recours

Toute erreur affichée propose systématiquement au moins une des actions suivantes, selon sa nature :
- réessayer l'opération,
- corriger la saisie à l'origine de l'erreur,
- accéder au **Support**,
- revenir à l'écran précédent ou à l'Accueil.

Une erreur sans aucune action de recours proposée est considérée comme incomplète.

## 6. Cohérence avec ORION

- Lorsqu'ORION ne peut répondre à une question (donnée indisponible, hors périmètre d'accès, cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4), la réponse l'indique explicitement plutôt que de formuler une réponse approximative ou incertaine présentée comme fiable.

## 7. Cas particulier — Alertes vs Erreurs

Une **Alerte** métier (écart de performance, cf. écran Alertes) n'est jamais présentée avec le vocabulaire ou les codes visuels d'une erreur système : les deux notions sont distinctes et ne doivent jamais être confondues dans leur traitement visuel, au risque de banaliser l'une ou de dramatiser l'autre.
