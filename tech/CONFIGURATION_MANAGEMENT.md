# ORBITA AI — Configuration Management

> Ce document définit la gestion technique des trois niveaux de configuration du système, déclinant en termes techniques les options fonctionnelles définies dans [features/SETTINGS.md](../features/SETTINGS.md).

## 1. Principe général

Trois niveaux de configuration, strictement distincts, coexistent dans le système, chacun avec sa propre portée et son propre responsable :

1. **Configuration système** — propre à l'instance déployée (Cloud ou On-Premise) de la plateforme.
2. **Configuration Organisation** — propre à chaque Organisation cliente.
3. **Configuration utilisateur** — propre à chaque utilisateur au sein d'une Organisation.

Aucun niveau ne doit jamais lire ou modifier directement un niveau supérieur ou inférieur sans passer par l'interface prévue à cet effet.

## 2. Configuration système

- Regroupe les paramètres nécessaires au fonctionnement de l'instance déployée : connexion à la base PostgreSQL (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md)), mode de déploiement (Cloud ou On-Premise, cf. [DEPLOYMENT_STRATEGY.md](DEPLOYMENT_STRATEGY.md)), canal de mise à jour actif (cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) §4).
- N'est jamais accessible depuis l'interface utilisateur standard : sa modification relève exclusivement de l'exploitation technique de la plateforme (équipe d'exploitation en Cloud, Administrateur système en On-Premise).
- Est chargée au démarrage de la plateforme et validée avant que celle-ci ne devienne accessible aux utilisateurs : une configuration système invalide empêche le démarrage plutôt que de provoquer un dysfonctionnement silencieux ultérieur.

## 3. Configuration Organisation

- Regroupe l'ensemble des paramètres définis dans [features/SETTINGS.md](../features/SETTINGS.md) §2 : objectifs, structure organisationnelle, seuils d'alerte, Grands Comptes, cartes KPI par défaut, langue par défaut.
- Gérée exclusivement par les Administrateurs de l'Organisation concernée, sans jamais être visible ou modifiable par une autre Organisation (cf. cloisonnement, [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4).
- Toute modification est journalisée (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3) et prend effet immédiatement, sans nécessiter de redémarrage de la plateforme.

## 4. Configuration utilisateur

- Regroupe les paramètres définis dans [features/SETTINGS.md](../features/SETTINGS.md) §3 : préférences personnelles, langue d'affichage, notifications, personnalisation du Dashboard.
- Propre à chaque utilisateur, jamais partagée ni visible par d'autres utilisateurs, y compris au sein de la même Organisation.
- Modifiable directement par l'utilisateur concerné, sans intervention d'un Administrateur, à l'exception des paramètres relevant du rôle ou du périmètre d'accès (cf. [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md) §8).

## 5. Précédence et résolution

- Une valeur de configuration utilisateur, lorsqu'elle existe, prévaut sur la valeur de configuration Organisation correspondante (ex. langue d'affichage) ; une valeur de configuration Organisation prévaut sur une valeur par défaut du système ; une configuration système ne peut jamais être outrepassée par un niveau inférieur.
- Cette hiérarchie de précédence est appliquée de façon strictement identique quel que soit le module concerné, sans exception locale.

## 6. Stockage et validation

- Chaque niveau de configuration est stocké de façon structurée et versionnée dans la base de données de référence (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md)), à l'exception de la configuration système qui peut être définie en dehors de la base pour rester disponible avant même la connexion à celle-ci.
- Toute configuration est validée à son enregistrement (cf. [architecture/ERROR_HANDLING.md](../architecture/ERROR_HANDLING.md) §2) : une valeur invalide est rejetée immédiatement, jamais acceptée puis source de dysfonctionnement différé.

## 7. Cohérence avec les éditions et licences

- La disponibilité de certaines options de configuration Organisation (ex. seuils d'alerte affinés par périmètre) dépend de la licence active (cf. [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md)) ; une option non incluse dans l'offre souscrite n'est pas présentée à l'Administrateur, plutôt que présentée mais inopérante.
