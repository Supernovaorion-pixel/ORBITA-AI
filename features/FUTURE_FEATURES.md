# ORBITA AI — Future Features

> Ce document recense les pistes fonctionnelles envisagées après la première version commerciale (V1) du produit. Ces pistes ne sont pas engagées : elles constituent un espace de réflexion documenté, à valider et spécifier formellement avant toute réalisation, conformément à [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §4. Aucune de ces pistes n'implique de choix technique.

## 1. Analyse et intelligence

- **Détection d'anomalies élargie** — extension de la détection au-delà des seuils déclaratifs actuels (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)) vers une identification de motifs inhabituels non anticipés par une règle explicite.
- **Simulation multi-scénarios avancée** — comparaison simultanée d'un nombre étendu de scénarios (cf. [FORECAST_ENGINE.md](FORECAST_ENGINE.md) §8), avec recommandation du scénario le plus probable au regard de l'évolution réelle constatée.
- **Analyse de cohortes clients** — suivi de groupes de Clients partageant une origine ou une caractéristique commune, au-delà de l'analyse individuelle actuelle (cf. [CLIENT_MANAGEMENT.md](CLIENT_MANAGEMENT.md)).
- **Benchmark inter-organisations anonymisé** — comparaison de la performance d'une Organisation à des références sectorielles agrégées et anonymisées, sans jamais exposer de donnée d'une Organisation à une autre.

## 2. ORION

- **Actions déclenchées depuis ORION** — au-delà des recommandations actuelles (cf. [ORION.md](ORION.md) §17), possibilité pour l'utilisateur de déclencher directement une action métier suggérée (ex. création d'une alerte de suivi personnalisée) depuis la conversation.
- **Mémoire de préférence utilisateur** — ORION affinant ses suggestions et son niveau de détail selon les habitudes de consultation propres à chaque utilisateur.
- **Explication comparative approfondie** — capacité à comparer explicitement deux périmètres ou deux périodes en une seule réponse structurée, au-delà de l'explication d'un seul constat à la fois.

## 3. Reporting et restitution

- **Rapports interactifs** — rapports consultables avec un niveau d'interactivité limité (changement de période sans régénération complète), au-delà des formats figés actuels (cf. [REPORTING_ENGINE.md](REPORTING_ENGINE.md)).
- **Modèles de rapport personnalisables par l'Organisation** — au-delà du catalogue officiel de modèles, possibilité pour une Organisation de composer ses propres modèles de restitution.
- **Partage externe sécurisé** — partage d'un rapport avec un destinataire extérieur à l'Organisation via un accès temporaire et restreint, sans création de compte utilisateur complet.

## 4. Extensibilité

- **Marketplace de plugins** — espace élargi de découverte et d'évaluation des plugins tiers disponibles, au-delà du catalogue restreint actuel (cf. [PLUGIN_SYSTEM.md](PLUGIN_SYSTEM.md)).
- **Connecteurs additionnels** — extension progressive du catalogue de connecteurs ERP/CRM (cf. [CONNECTORS.md](CONNECTORS.md)) à de nouveaux systèmes sources, selon la demande des Organisations clientes.
- **API ouverte** — exposition formelle d'une interface de programmation permettant à des systèmes tiers d'interagir avec ORBITA AI, anticipée par les principes d'architecture définis dans [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §10.

## 5. Collaboration

- **Annotations et commentaires** — possibilité pour les utilisateurs d'annoter un indicateur, un graphique ou une alerte à l'intention d'autres utilisateurs habilités de l'Organisation, pour documenter un constat ou une décision.
- **Espaces de travail partagés** — regroupement de vues et de rapports autour d'un sujet commun (ex. revue trimestrielle d'un territoire), consultable par les utilisateurs concernés.

## 6. Internationalisation

- **Extension du catalogue de langues** — au-delà des langues initialement supportées, ajout progressif de nouvelles langues d'interface, sans modification du Domaine (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §7).
- **Adaptation aux devises et normes régionales multiples** — prise en charge de plusieurs devises et conventions de présentation numérique au sein d'une même Organisation multi-pays.

## 7. Mobilité

- **Application mobile dédiée** — au-delà de l'adaptation responsive actuelle (cf. [ux/RESPONSIVE_RULES.md](../ux/RESPONSIVE_RULES.md)), une expérience mobile native pensée pour la consultation rapide et les notifications en situation de mobilité.

## 8. Gouvernance et conformité

- **Certifications et conformités sectorielles étendues** — accompagnement de certaines Organisations Enterprise vers des exigences de conformité spécifiques à leur secteur d'activité, au-delà des principes généraux définis dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md).
- **Délégation d'administration fine** — répartition des droits d'Administration entre plusieurs Administrateurs selon des périmètres distincts (ex. un Administrateur par filiale), au-delà du rôle Administrateur unique actuel (cf. [USER_MANAGEMENT.md](USER_MANAGEMENT.md)).

## 9. Principe d'évaluation de ces pistes

Aucune piste de ce document ne doit être engagée sans, au préalable : une spécification fonctionnelle formelle équivalente aux documents de ce dossier `features/`, une vérification de cohérence avec l'architecture existante ([architecture/](../architecture/)) et une évaluation de sa place dans la [FEATURE_MATRIX.md](FEATURE_MATRIX.md) par offre commerciale.
