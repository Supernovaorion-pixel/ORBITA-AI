# ORBITA AI — Technology Stack

> Ce document fixe le socle technologique officiel et définitif d'ORBITA AI. Chaque choix est justifié au regard de cinq critères non négociables : **pérennité**, **performances**, **communauté**, **maintenance** et **lisibilité**. Ces choix doivent rester valides au moins dix ans et servir toutes les éditions (Community à Enterprise) et tous les modes de déploiement (Cloud, On-Premise) à partir d'une base de code unique.

## 1. Méthode de décision

Aucune technologie n'est retenue par préférence ou par tendance du moment. Chaque choix ci-dessous répond à une question simple : *cette technologie sera-t-elle encore maintenue, documentée et employable dans dix ans, par une communauté suffisamment large pour ne jamais dépendre d'un seul acteur ?* Une technologie qui ne résiste pas à cette question est écartée, quelle que soit sa popularité actuelle.

## 2. Langage et runtime applicatif — .NET (C#), versions LTS

**Décision** : le socle applicatif (Core et l'ensemble des modules définis dans [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md)) est écrit en C#, exécuté sur les versions à support long terme (LTS) de .NET.

**Justification** :
- **Pérennité** — .NET est un socle multiplateforme mature, avec un cycle de versions LTS prévisible et un engagement de support à long terme ; l'écosystème existe depuis plus de vingt ans et continue d'évoluer sans rupture de compatibilité majeure.
- **Performances** — runtime compilé, à typage statique, offrant des performances adaptées aux volumes visés par [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) (plusieurs millions de lignes, calculs analytiques complexes).
- **Communauté** — l'une des communautés de développement d'entreprise les plus larges et actives, garantissant disponibilité de compétences et pérennité de la documentation.
- **Maintenance** — outillage de diagnostic, de profilage et de gestion des dépendances mature et standardisé.
- **Lisibilité** — typage statique fort, cohérent avec les exigences de lisibilité et de maintenabilité définies dans [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md).

**Cohérence multiplateforme** : .NET s'exécute nativement sur Windows (plateforme prioritaire), Linux et macOS à partir d'un seul et même code source, condition indispensable à la règle de base de code unique.

## 3. Interface utilisateur — TypeScript + React

**Décision** : la couche Présentation (cf. [architecture/APPLICATION_LAYERS.md](../architecture/APPLICATION_LAYERS.md)) est développée en TypeScript, avec React comme bibliothèque de construction d'interface.

**Justification** :
- **Pérennité** — TypeScript est devenu un standard de fait du développement d'interface d'entreprise ; React bénéficie d'un support et d'une adoption industrielle qui garantissent sa maintenance à long terme.
- **Performances** — rendu par composants, adapté à la densité d'information exigée par [ux/DASHBOARD_SPECIFICATION.md](../ux/DASHBOARD_SPECIFICATION.md) sans dégradation perceptible.
- **Communauté** — écosystème le plus large du marché pour la construction d'interfaces web d'entreprise, garantissant disponibilité de compétences et de bibliothèques compatibles.
- **Maintenance** — le typage statique de TypeScript réduit la classe d'erreurs la plus courante des interfaces web (incohérence de type entre couches).
- **Lisibilité** — modèle par composants directement transposable au [ux/COMPONENT_LIBRARY.md](../ux/COMPONENT_LIBRARY.md), un composant fonctionnel correspondant à un composant technique.

**Accès multiplateforme** : l'interface étant restituée dans un navigateur web standard, elle est nativement accessible depuis Windows, Linux et macOS sans développement spécifique par plateforme.

## 4. Stockage de données — PostgreSQL

**Décision** : le stockage de référence de la plateforme repose sur PostgreSQL. Justification complète dans [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md).

**Synthèse** : moteur relationnel open source, sans coût de licence bloquant pour les déploiements On-Premise, avec vingt-cinq ans de maturité, une communauté considérable, et des capacités analytiques (agrégation, fenêtrage) directement pertinentes pour l'Analytics Engine et le Forecast Engine.

## 5. Communication inter-modules

- **En-processus (par défaut)** — les modules définis dans [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) s'exécutent au sein d'un même processus applicatif .NET, communiquant par interfaces stables (cf. [architecture/APPLICATION_LAYERS.md](../architecture/APPLICATION_LAYERS.md)), pour un déploiement simple compatible avec toutes les éditions, y compris Community en On-Premise restreint.
- **Bus d'événements interne, extensible vers un courtier de messages standard (AMQP)** — le système d'événements défini dans [architecture/EVENT_SYSTEM.md](../architecture/EVENT_SYSTEM.md) est implémenté en mémoire par défaut, avec une interface d'abstraction permettant son adossement à un courtier de messages conforme au standard ouvert AMQP pour les déploiements Business/Enterprise à forte charge, sans jamais imposer cette dépendance aux éditions plus légères.

## 6. Format d'échange

- **JSON** comme format d'échange de données entre la Présentation et l'Application, et pour toute API (cf. [API_STRATEGY.md](API_STRATEGY.md)) : format universellement supporté, lisible, à la fois par des humains lors du diagnostic et par toute plateforme cliente future.

## 7. Ce qui est explicitement écarté

- **Tout langage ou runtime propriétaire à license restrictive** — incompatible avec l'exigence de coût maîtrisé pour les éditions Community/Starter et les déploiements On-Premise.
- **Tout framework d'interface expérimental ou récent sans historique de maintenance** — le risque d'abandon avant l'horizon de dix ans est jugé inacceptable pour une plateforme destinée à durer.
- **Toute base de données propriétaire à coût de licence élevé** — incompatible avec l'objectif de coût maîtrisé pour toutes les éditions, en particulier Community et Starter.

## 8. Cohérence transversale

Chaque choix de ce document est repris et détaillé dans les documents spécialisés du présent dossier ([DATABASE_STRATEGY.md](DATABASE_STRATEGY.md), [API_STRATEGY.md](API_STRATEGY.md), [AI_INTEGRATION.md](AI_INTEGRATION.md), etc.). Aucun de ces documents ne doit introduire une technologie non alignée avec le présent socle sans mise à jour explicite et justifiée de ce document (cf. principe de décision documentée, [architecture/ARCHITECTURE_DECISIONS.md](../architecture/ARCHITECTURE_DECISIONS.md)).
