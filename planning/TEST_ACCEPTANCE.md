# ORBITA AI — Test Acceptance

> Ce document définit les critères de validation utilisateur (acceptation) applicables aux livrables du projet, complémentaires aux vérifications techniques définies dans [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md). Là où la stratégie de test vérifie que le système fonctionne comme spécifié, l'acceptation vérifie qu'il répond réellement au besoin de l'utilisateur final.

## 1. Principe général

Un livrable n'est accepté que s'il permet à l'un des profils définis dans [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md) d'accomplir le cas d'utilisation qui lui est associé, dans les conditions réelles d'usage prévues, sans confusion ni effort disproportionné.

## 2. Critères d'acceptation par nature de livrable

### 2.1 Module fonctionnel (Import, Analytics, Forecast, Reporting, etc.)
- Le comportement observé correspond exactement à la spécification du document [features/](../features/) correspondant, sans interprétation divergente.
- Un utilisateur représentatif du persona concerné parvient à réaliser le cas d'utilisation associé sans assistance extérieure au produit lui-même (aide contextuelle, ORION).

### 2.2 Écran ou parcours (interface utilisateur)
- L'écran respecte la structure définie dans [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) pour l'écran concerné.
- Le parcours correspondant, s'il existe, se déroule conformément à [ux/USER_FLOWS.md](../ux/USER_FLOWS.md), sans étape manquante ni redondante.
- Les règles d'accessibilité définies dans [ux/ACCESSIBILITY_UI.md](../ux/ACCESSIBILITY_UI.md) sont vérifiées par un test de navigation clavier complet et un contrôle de contraste.

### 2.3 ORION
- Une question type posée par chaque persona concerné reçoit une réponse conforme aux capacités définies dans [features/ORION.md](../features/ORION.md), et le module refuse explicitement de répondre lorsque la question dépasse ses limites fonctionnelles (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4).

### 2.4 Édition commerciale (Community à Enterprise)
- Chaque édition, testée isolément, expose exactement le périmètre défini dans [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md), ni plus, ni moins.

### 2.5 Mode de déploiement (Cloud, On-Premise)
- Le comportement fonctionnel observé est strictement identique entre les deux modes, à l'exception des différences explicitement documentées (cf. [tech/DEPLOYMENT_STRATEGY.md](../tech/DEPLOYMENT_STRATEGY.md) §3).

## 3. Modalités de validation

- L'acceptation d'un module ou d'un écran est réalisée par une personne distincte de son développeur, jouant le rôle du persona concerné à partir de son cas d'utilisation documenté, sans connaissance préalable de l'implémentation interne.
- Une acceptation est enregistrée formellement (module, date, persona simulé, résultat), constituant une preuve consultable lors de la vérification des [QUALITY_GATES.md](QUALITY_GATES.md) de la phase concernée.
- Un rejet en acceptation renvoie le module en développement ; il ne peut jamais être compensé par une dérogation ponctuelle.

## 4. Acceptation à l'échelle du système (Phase 14 — Tests globaux)

- L'ensemble des grands parcours définis dans [ux/USER_FLOWS.md](../ux/USER_FLOWS.md) §1-7 est rejoué de bout en bout, sur une instance représentative de chaque édition commerciale.
- Un scénario représentatif de chaque persona de [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md) est exécuté intégralement sans assistance extérieure au produit.

## 5. Acceptation avant chaque jalon de publication

- Avant la Version Beta (cf. [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md)), l'acceptation est réalisée en interne selon les mêmes critères que ceux définis ici.
- Pendant la Version Beta, l'acceptation est complétée par le retour direct d'Organisations clientes volontaires, dont les remarques sont traitées selon la même rigueur qu'un rejet d'acceptation interne.
- Avant la Version Stable, aucune anomalie d'acceptation non résolue ne peut subsister, qu'elle provienne des tests internes ou des retours de la phase Beta.

## 6. Principe transversal

Un module peut satisfaire intégralement [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) et échouer son acceptation si son comportement, bien que techniquement correct, ne sert pas réellement l'utilisateur dans son contexte réel d'usage. L'acceptation prévaut toujours sur la seule conformité technique.
