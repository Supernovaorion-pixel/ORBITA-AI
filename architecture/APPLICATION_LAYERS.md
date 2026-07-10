# ORBITA AI — Application Layers

> Ce document définit les couches logiques que chaque module doit respecter. Aucune implémentation, aucun framework n'est prescrit.

## 1. Principe des couches

Chaque module fonctionnel (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md)) est structuré selon les mêmes couches logiques, appliquées de façon cohérente à travers tout le système. Cette uniformité est ce qui permet à un contributeur de comprendre n'importe quel module en ayant compris la structure d'un seul.

## 2. Les couches

### Présentation
- Responsable de la restitution de l'information à l'utilisateur et de la capture de ses actions.
- Ne contient aucune règle métier : elle traduit une intention utilisateur en appel à la couche Application, et un résultat en restitution compréhensible.
- Interchangeable par nature : une même logique d'Application doit pouvoir être restituée par plusieurs présentations (interface graphique, export, API future) sans être dupliquée.

### Application
- Orchestre les cas d'usage : elle coordonne les entités du Domaine et les services nécessaires pour répondre à une action utilisateur ou système.
- Ne contient pas la règle métier elle-même (portée par le Domaine), mais la séquence dans laquelle ces règles sont invoquées.
- Point d'entrée unique vers un module pour toute couche Présentation.

### Domaine
- Porte la connaissance métier fondamentale : les entités définies dans [DOMAIN_MODEL.md](DOMAIN_MODEL.md), leurs relations et les règles qui leur sont intrinsèques (ex. : une Facture ne peut référencer un Client que de la même Organisation).
- Ne dépend d'aucune autre couche : c'est la couche la plus stable du système, celle qui doit rester valide même si la Présentation ou l'Infrastructure changent entièrement.
- Ignore tout ce qui relève du stockage, de l'affichage ou de l'environnement d'exécution.

### Infrastructure
- Relie le système au monde extérieur : sources de données externes, environnement d'exécution, connecteurs (ERP, CRM), mécanismes de persistance.
- Implémente les interfaces définies par le Domaine et l'Application, sans jamais leur imposer ses propres contraintes.
- Couche la plus susceptible de changer dans le temps (changement d'environnement, nouveau connecteur) ; c'est pourquoi elle ne doit jamais être connue directement du Domaine.

### Services
- Regroupe les capacités transversales utilisées par plusieurs modules (sécurité, journalisation, notification, gestion des licences) sans appartenir en propre au Domaine d'un module particulier.
- Accessible par l'Application de chaque module à travers une interface stable, jamais par accès direct à l'implémentation.

## 3. Règle de dépendance entre couches

```
Présentation → Application → Domaine ← Infrastructure
                     ↓
                  Services
```

- Une couche ne peut dépendre que des couches situées "plus près" du Domaine, jamais l'inverse.
- Le **Domaine ne dépend de rien** : ni de la Présentation, ni de l'Infrastructure, ni des Services. C'est l'Infrastructure qui s'adapte au Domaine (par l'implémentation d'interfaces définies par celui-ci), jamais l'inverse.
- Cette règle est ce qui permet de changer d'environnement d'exécution ou de source de données (Infrastructure) sans toucher à la logique métier (Domaine) — condition essentielle de la pérennité à dix ans de l'architecture.

## 4. Application aux éditions et modes de déploiement

- La distinction entre édition (Community à Enterprise) et mode de déploiement (Cloud, On-Premise) relève exclusivement de la couche **Infrastructure** et de la configuration des **Services** (notamment Licensing) : le Domaine et l'Application restent strictement identiques quelle que soit l'édition ou le mode de déploiement.
- C'est ce principe qui garantit qu'une même base logique sert toutes les éditions sans fork ni duplication de code métier.

## 5. Cohérence avec les principes de codage

Cette structure en couches est indissociable des principes définis dans [CODING_PRINCIPLES.md](CODING_PRINCIPLES.md), notamment la séparation des responsabilités (SOLID) et l'inversion de dépendance qui permet au Domaine de rester indépendant de l'Infrastructure.
