# ORBITA AI — Folder Structure

> Ce document décrit l'arborescence officielle **future** du projet, à titre de référence documentaire uniquement. Aucun dossier ni fichier de code n'est créé à ce stade — cette structure encadrera la future implémentation.

## 1. Principe directeur

L'arborescence reflète directement la structure logique définie dans [SYSTEM_ARCHITECTURE.md](SYSTEM_ARCHITECTURE.md), [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) et [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md) : un dossier par module, chaque module structuré selon les mêmes couches. Un contributeur qui connaît cette structure sait retrouver n'importe quel élément du système sans avoir à en explorer le contenu.

## 2. Arborescence de référence (future)

```
orbita-ai/
├── branding/                  # Identité de marque officielle (existant)
├── docs/                      # Documentation fondatrice du projet (existant)
├── architecture/              # Documentation d'architecture (existant)
│
├── core/                      # Module Core — socle commun, Domaine partagé
│   ├── domain/                # Entités et règles métier transversales
│   ├── application/           # Cas d'usage communs
│   ├── services/               # Services transversaux (config, identité)
│   └── infrastructure/        # Adaptation à l'environnement d'exécution
│
├── modules/                   # Un sous-dossier par module fonctionnel
│   ├── import-engine/
│   │   ├── domain/
│   │   ├── application/
│   │   ├── infrastructure/
│   │   └── presentation/
│   ├── analytics-engine/
│   │   ├── domain/
│   │   ├── application/
│   │   ├── infrastructure/
│   │   └── presentation/
│   ├── forecast-engine/
│   ├── reporting-engine/
│   ├── dashboard/
│   ├── orion/
│   ├── licensing/
│   ├── administration/
│   ├── settings/
│   ├── security/
│   ├── update-manager/
│   ├── plugin-system/
│   ├── export-engine/
│   ├── notification-center/
│   ├── history/
│   └── audit/
│
├── plugins/                   # Extensions et connecteurs tiers (cf. EXTENSIBILITY.md)
│   ├── connectors/             # Connecteurs ERP / CRM
│   └── extensions/             # Moteurs, rapports ou dashboards additionnels
│
└── shared/                    # Éléments partagés non porteurs de règle métier
    ├── localization/           # Ressources multilingues
    └── contracts/              # Interfaces partagées entre modules
```

## 3. Règles de structure interne d'un module

Chaque dossier de `modules/<nom-du-module>/` respecte les mêmes sous-dossiers, correspondant aux couches définies dans [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md) :

- `domain/` — entités et règles propres au module.
- `application/` — orchestration des cas d'usage du module.
- `infrastructure/` — adaptation aux sources de données et connecteurs externes au module.
- `presentation/` — restitution spécifique au module (uniquement si le module en possède une propre).

Un module qui ne possède pas de couche donnée (par exemple, `history` sans présentation propre) omet simplement le dossier correspondant, sans créer de dossier vide.

## 4. Emplacement des connecteurs et plugins

- Les connecteurs ERP/CRM et extensions futures ne résident jamais dans `modules/`, mais exclusivement dans `plugins/`, conformément à l'indépendance du noyau vis-à-vis des extensions (cf. [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md) §7).
- Cette séparation garantit qu'une édition Community peut être distribuée sans le dossier `plugins/`, sans que cela n'affecte la structure du noyau.

## 5. Stabilité de la structure

- L'ajout d'un nouveau module fonctionnel se traduit uniquement par l'ajout d'un nouveau sous-dossier dans `modules/`, jamais par une modification de la structure des modules existants.
- L'ajout d'un plugin, connecteur ou d'une langue se traduit uniquement par un ajout dans `plugins/` ou `shared/localization/`, jamais par une modification du noyau.
- Cette arborescence est conçue pour rester stable même après plusieurs années d'évolution du produit, conformément à l'exigence de pérennité de l'architecture.
