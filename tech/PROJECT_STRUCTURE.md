# ORBITA AI — Project Structure

> Ce document décrit l'arborescence technique future du projet, déclinant en termes concrets la structure logique définie dans [architecture/FOLDER_STRUCTURE.md](../architecture/FOLDER_STRUCTURE.md) selon le socle technologique retenu dans [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md). Aucun dossier ni fichier n'est créé à ce stade : ce document sert de référence à la future mise en place du dépôt de code.

## 1. Principe

L'arborescence technique reflète directement l'arborescence logique déjà définie, en y ajoutant les éléments propres à l'écosystème .NET (solution, projets) et à l'écosystème frontend (espace de travail Node.js).

## 2. Arborescence technique de référence

```
orbita-ai/
├── branding/                      # (existant — identité de marque)
├── docs/                          # (existant — documentation fondatrice)
├── architecture/                  # (existant — documentation d'architecture)
├── ux/                            # (existant — documentation UX/UI)
├── features/                      # (existant — spécifications fonctionnelles)
├── tech/                          # (existant — présent dossier)
│
├── backend/
│   ├── OrbitaAI.sln                # Solution regroupant l'ensemble des projets .NET
│   ├── src/
│   │   ├── Core/                   # Projet Core (Domaine et services transversaux)
│   │   ├── Modules/
│   │   │   ├── ImportEngine/
│   │   │   ├── AnalyticsEngine/
│   │   │   ├── ForecastEngine/
│   │   │   ├── ReportingEngine/
│   │   │   ├── Dashboard/
│   │   │   ├── Orion/
│   │   │   ├── Licensing/
│   │   │   ├── Administration/
│   │   │   ├── Settings/
│   │   │   ├── Security/
│   │   │   ├── UpdateManager/
│   │   │   ├── PluginSystem/
│   │   │   ├── ExportEngine/
│   │   │   ├── NotificationCenter/
│   │   │   ├── History/
│   │   │   └── Audit/
│   │   └── Host/                   # Point d'assemblage exécutable de la plateforme
│   └── tests/
│       ├── UnitTests/
│       ├── IntegrationTests/
│       └── FunctionalTests/
│
├── frontend/
│   ├── package.json                # Espace de travail racine (workspace)
│   ├── apps/
│   │   └── web/                    # Application de présentation principale
│   ├── packages/
│   │   ├── design-system/          # Composants issus de ux/COMPONENT_LIBRARY.md
│   │   └── shared/                 # Utilitaires partagés entre applications frontend
│   └── tests/
│       ├── unit/
│       └── ui/
│
├── plugins/
│   ├── connectors/                 # Connecteurs ERP/CRM (cf. features/CONNECTORS.md)
│   └── extensions/                 # Extensions tierces (cf. features/PLUGIN_SYSTEM.md)
│
├── shared/
│   ├── localization/                # Ressources multilingues
│   └── contracts/                   # Schémas d'interface partagés backend/frontend
│
└── deployment/
    ├── cloud/                       # Manifestes de déploiement Cloud
    └── on-premise/                  # Programmes d'installation On-Premise (cf. BUILD_AND_PACKAGING.md)
```

## 3. Correspondance avec l'architecture logique

- Chaque dossier de `backend/src/Modules/` correspond exactement à un module défini dans [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md), structuré en interne selon les couches définies dans [architecture/APPLICATION_LAYERS.md](../architecture/APPLICATION_LAYERS.md) (Domaine, Application, Infrastructure).
- Le projet `Core` ne dépend d'aucun module, conformément à [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) ; tout projet de module référence `Core` mais jamais l'inverse.
- Le projet `Host` assemble l'ensemble des modules natifs activés selon la licence (cf. [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md)) en un exécutable unique pour un déploiement en processus unique, tout en conservant la possibilité d'isoler un module à forte charge si un déploiement Enterprise le justifie.

## 4. Séparation backend / frontend

- `backend/` et `frontend/` constituent deux espaces de compilation distincts au sein d'un même dépôt de code unique (mono-dépôt), garantissant une gestion de version commune et cohérente entre les deux couches, conformément à l'exigence d'une seule base de code.
- Le dossier `shared/contracts/` porte les définitions d'interface (structure des messages échangés) utilisées par les deux couches, garantissant leur cohérence sans duplication (cf. principe DRY, [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §3).

## 5. Tests

- Chaque niveau de test défini dans [TESTING_STRATEGY.md](TESTING_STRATEGY.md) dispose de son propre dossier dédié, backend et frontend, jamais mélangé au code de production.

## 6. Stabilité de la structure

- L'ajout d'un nouveau module fonctionnel se traduit exclusivement par l'ajout d'un nouveau projet dans `backend/src/Modules/`, sans modification de la structure des modules existants, conformément à [architecture/FOLDER_STRUCTURE.md](../architecture/FOLDER_STRUCTURE.md) §5.
- L'ajout d'un connecteur ou d'une extension se traduit exclusivement par un ajout dans `plugins/`, jamais par une modification de `backend/src/Modules/` ou `frontend/apps/`.
