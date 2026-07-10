# ORBITA AI — Architecture Decision Records (ADR)

> Ce document consigne les décisions d'architecture structurantes du projet, avec leur justification. Chaque nouvelle décision significative doit être ajoutée ici selon le même format, jamais actée sans être documentée (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §3).

## Format d'une décision

Chaque décision est consignée selon la structure : **Contexte** (pourquoi la question se pose), **Décision** (ce qui est retenu), **Justification** (pourquoi ce choix plutôt qu'une alternative), **Conséquences** (ce que cela implique pour la suite).

---

## ADR-001 — Architecture modulaire à noyau central

**Contexte** : le système doit décliner plusieurs éditions commerciales (Community à Enterprise), fonctionner en Cloud et On-Premise, et accueillir des extensions futures, sans être reconstruit à chaque évolution.

**Décision** : adopter une architecture composée d'un noyau (`Core`) et de modules fonctionnels indépendants (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md)), communiquant via interfaces stables et événements plutôt que par dépendances directes.

**Justification** : une architecture monolithique non modulaire imposerait de dupliquer ou de conditionner le code pour chaque édition commerciale, et rendrait toute extension future risquée pour l'ensemble du système. La modularité permet d'activer/désactiver des modules selon la licence sans dupliquer la base de code (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md)).

**Conséquences** : chaque nouveau module doit respecter les règles de dépendance de [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md) ; la discipline de séparation a un coût de rigueur initial, compensé par la stabilité à long terme du système.

---

## ADR-002 — Séparation stricte en couches applicatives

**Contexte** : le système doit pouvoir évoluer d'environnement d'exécution (Cloud vers On-Premise, ou évolution technologique future) sans réécrire la logique métier.

**Décision** : structurer chaque module selon les couches Présentation / Application / Domaine / Infrastructure / Services, le Domaine ne dépendant jamais de l'Infrastructure (cf. [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md)).

**Justification** : c'est la seule façon de garantir qu'un changement d'environnement d'exécution ou de source de données n'affecte pas la valeur durable du système, à savoir sa connaissance du métier commercial. Une architecture qui mêlerait Domaine et Infrastructure obligerait à revalider l'ensemble de la logique métier à chaque changement technique.

**Conséquences** : toute implémentation future de l'Infrastructure doit se conformer aux interfaces définies par le Domaine, jamais l'inverse ; cela impose une discipline de conception rigoureuse dès les premières étapes de développement.

---

## ADR-003 — Communication inter-modules par événements

**Contexte** : les modules doivent rester indépendants pour permettre l'ajout, le remplacement ou la désactivation de l'un d'entre eux sans effet de bord sur les autres.

**Décision** : privilégier un système d'événements (cf. [EVENT_SYSTEM.md](EVENT_SYSTEM.md)) pour toute communication qui ne relève pas d'une dépendance fonctionnelle stable et assumée.

**Justification** : un appel direct entre modules crée un couplage qui complique toute évolution ultérieure et empêche l'ajout silencieux d'un nouveau module réagissant à un fait existant (ex. un futur moteur d'IA additionnel). Le modèle événementiel permet cette extension sans modifier le module émetteur.

**Conséquences** : la traçabilité des événements devient une exigence de premier ordre (cf. [LOGGING_STRATEGY.md](LOGGING_STRATEGY.md)) ; la conception de chaque module doit désormais distinguer explicitement ce qui relève d'un appel direct de ce qui relève d'un événement.

---

## ADR-004 — Cloisonnement strict par Organisation au niveau du Domaine

**Contexte** : le système doit héberger plusieurs organisations clientes, sans qu'aucune fuite de donnée ne soit possible entre elles, quel que soit le niveau de licence ou de configuration.

**Décision** : faire du cloisonnement par Organisation un invariant du Domaine lui-même (cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md) §4), et non une règle appliquée uniquement au niveau de la sécurité applicative.

**Justification** : une séparation appliquée uniquement en périphérie (sécurité, accès) laisse la possibilité d'un contournement si une couche inférieure ne la respecte pas. En l'ancrant dans le Domaine, aucune relation entre entités ne peut techniquement franchir la frontière d'une Organisation, quelle que soit la couche qui y accède.

**Conséquences** : toute nouvelle entité de Domaine doit explicitement se rattacher à une Organisation dès sa conception ; cela renforce la confidentialité exigée par [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §1 au niveau structurel plutôt que procédural.

---

## ADR-005 — Extensions et connecteurs isolés du noyau via un Plugin System

**Contexte** : le système doit accueillir des connecteurs ERP/CRM, de nouveaux moteurs d'IA, de nouveaux rapports et dashboards, sans que ces ajouts ne remettent en cause la stabilité du noyau.

**Décision** : faire porter toute extension par un module dédié, `Plugin System`, dépendant uniquement du `Core` et du `Licensing`, jamais l'inverse (cf. [EXTENSIBILITY.md](EXTENSIBILITY.md), [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md) §7).

**Justification** : si les modules natifs dépendaient des extensions pour fonctionner, la moindre extension tierce deviendrait un point de fragilité pour l'ensemble du système. En inversant cette dépendance, le noyau reste fonctionnel et stable indépendamment des extensions installées.

**Conséquences** : chaque module natif doit exposer des points d'extension explicites et volontaires ; une extension ne peut jamais accéder à autre chose que ce que ces points exposent.

---

## ADR-006 — Semantic Versioning avec canaux de maturité et LTS

**Contexte** : le système est distribué à la fois en Cloud (mise à jour continue) et en On-Premise (mise à jour maîtrisée par l'Organisation cliente), sur une durée de vie visée de dix ans.

**Décision** : adopter le Semantic Versioning, complété par des canaux de maturité (Alpha, Beta, RC, Stable) et des versions LTS dédiées aux déploiements exigeant une stabilité prolongée (cf. [VERSIONING_STRATEGY.md](VERSIONING_STRATEGY.md)).

**Justification** : les organisations en On-Premise ou en licence Enterprise ont besoin d'une visibilité claire sur la nature et le risque de chaque mise à jour avant de l'appliquer à leur propre rythme ; un schéma de version ambigu compromettrait leur confiance dans le produit.

**Conséquences** : toute rupture de compatibilité doit être un choix explicite et documenté, jamais une conséquence accidentelle d'un changement mineur ; le support de versions LTS impose une discipline de maintenance différenciée entre lignes de version.

---

## ADR-007 — Documentation et principes de conception comme fondation avant tout développement

**Contexte** : le projet vise une pérennité d'au moins dix ans, avec une équipe appelée à évoluer dans le temps.

**Décision** : faire de la documentation d'architecture (ce dossier) et des principes de conception ([CODING_PRINCIPLES.md](CODING_PRINCIPLES.md), [NAMING_CONVENTIONS.md](NAMING_CONVENTIONS.md)) une référence qui précède et encadre tout développement futur, conformément à [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §4.

**Justification** : un projet destiné à durer une décennie ne peut reposer sur une connaissance implicite détenue par ses seuls concepteurs initiaux ; la documentation constitue la mémoire durable du système, indépendante des personnes qui y contribuent au fil du temps.

**Conséquences** : toute évolution future du système doit d'abord être confrontée à cette documentation avant d'être implémentée ; toute contradiction entre une pratique de développement et cette documentation doit être résolue en faveur de la documentation, ou entraîner une mise à jour explicite et justifiée de celle-ci.
