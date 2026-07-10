# ORBITA AI — Empty States

> Ce document définit les règles officielles de conception des états vides du produit. Aucune maquette n'est produite ici — uniquement les règles de contenu et de comportement.

## 1. Principe général

Un état vide n'est jamais un simple espace blanc ou un tableau sans ligne : c'est une opportunité d'orienter l'utilisateur vers l'action qui résoudrait ce vide. Chaque état vide doit répondre à trois questions : *pourquoi c'est vide*, *est-ce normal*, *que puis-je faire*.

## 2. Catégories d'état vide

### 2.1 Absence de donnée initiale (première utilisation)
- Survient lorsqu'une Organisation n'a pas encore réalisé d'Import ou de configuration (ex. Dashboard avant le premier import, écran Connecteurs sans connecteur configuré).
- Contenu attendu : un message expliquant que l'écran s'activera dès qu'une donnée sera disponible, accompagné d'une action directe vers l'écran ou la démarche qui produira cette donnée (ex. vers **Import** ou **Connecteurs**).

### 2.2 Absence de résultat suite à un filtre
- Survient lorsqu'un filtre ou une recherche ne retourne aucun résultat (ex. tableau Clients filtré sur un critère trop restrictif).
- Contenu attendu : indication claire que le vide résulte du filtre actif (jamais confondu avec une absence réelle de donnée), avec une action immédiate pour réinitialiser ou ajuster le filtre.

### 2.3 Absence de contenu généré par l'utilisateur
- Survient lorsqu'aucun Rapport, Export ou Alerte n'a encore été créé.
- Contenu attendu : invitation directe à créer le premier élément, avec l'action correspondante immédiatement accessible depuis l'état vide lui-même.

### 2.4 Situation positive représentée par un vide
- Survient lorsque l'absence de contenu est une bonne nouvelle (ex. écran Alertes sans alerte active).
- Contenu attendu : message positif et clair confirmant l'absence d'anomalie, sans inviter à une action inutile — un vide positif ne doit jamais être présenté avec la même tonalité neutre qu'un vide nécessitant une action.

## 3. Règles de contenu

- Le message d'un état vide est toujours factuel et bref (une phrase), jamais familier ou ludique, conformément à la personnalité de marque (cf. [branding/BRAND_GUIDELINES.md](../branding/BRAND_GUIDELINES.md) §5).
- Une icône simple et sobre (cf. [branding/ICONOGRAPHY.md](../branding/ICONOGRAPHY.md)) accompagne le message, jamais une illustration figurative ou décorative.
- Lorsqu'une action est possible, elle est toujours présentée sous forme de bouton explicite, jamais uniquement suggérée par le texte.

## 4. Cas particulier — ORION

- Le panneau ORION, avant toute question posée, n'affiche jamais un état "vide" au sens strict : il présente systématiquement des suggestions contextuelles (cf. [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md) §20), l'absence d'historique de conversation n'étant jamais perçue comme un manque.

## 5. Cas particulier — Dashboard

- Un Dashboard sans donnée suffisante pour une carte KPI donnée (ex. Organisation trop récente pour calculer une Croissance annuelle) affiche cette carte dans un état vide propre à l'indicateur, sans faire disparaître la carte ni bloquer l'affichage des autres cartes disposant de données suffisantes.

## 6. Principe transversal

Un état vide ne doit jamais être traité comme un cas d'erreur (cf. [ERROR_STATES.md](ERROR_STATES.md)) : l'absence de donnée est une situation normale et anticipée du cycle de vie du produit, jamais un dysfonctionnement.
