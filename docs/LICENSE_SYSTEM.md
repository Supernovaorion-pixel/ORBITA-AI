# ORBITA AI — License System

> Ce document décrit le fonctionnement attendu du système de licences, du point de vue de ses règles et de son comportement. Aucune implémentation technique n'est décrite ici.

## 1. Objectif du système de licences

Le système de licences a pour rôle de faire correspondre, pour chaque organisation cliente, l'usage réel de la plateforme à l'offre commerciale souscrite (cf. [BUSINESS_MODEL.md](BUSINESS_MODEL.md)). Il garantit que chaque organisation accède exactement au périmètre fonctionnel et aux volumes convenus, ni plus, ni moins.

## 2. Structure d'une licence

Une licence ORBITA AI est attribuée à une organisation cliente et définit :

- **l'offre souscrite** (Community, Starter, Business, Enterprise),
- **le mode de déploiement** associé (Cloud ou On-Premise),
- **le périmètre fonctionnel** activé (modules accessibles, cf. [FUNCTIONAL_SPECIFICATION.md](FUNCTIONAL_SPECIFICATION.md)),
- **les volumes autorisés** (nombre d'utilisateurs, volume de données pris en charge),
- **la période de validité** de la licence (durée de l'abonnement, dates de renouvellement).

## 3. Cycle de vie d'une licence

1. **Attribution** : une licence est créée lors de la souscription d'une organisation à une offre.
2. **Activation** : la licence est activée pour l'organisation, rendant accessible le périmètre correspondant.
3. **Suivi d'usage** : l'usage réel de l'organisation (utilisateurs actifs, volumes de données) est comparé en continu aux limites de la licence.
4. **Renouvellement** : à échéance, la licence est reconduite selon les termes convenus, ou ajustée en cas de changement d'offre.
5. **Évolution** : une organisation peut faire évoluer sa licence à tout moment (montée en gamme, ajout d'utilisateurs), l'évolution prenant effet sans interruption de service.
6. **Expiration ou résiliation** : à l'échéance sans renouvellement, ou en cas de résiliation, l'accès à la plateforme est suspendu selon les conditions contractuelles applicables, sans perte des données de l'organisation pendant la période de grâce convenue.

## 4. Dépassement de limites

Lorsqu'une organisation approche ou dépasse une limite de sa licence (nombre d'utilisateurs, volume de données), le système doit :

- signaler la situation de façon claire à l'administrateur de l'organisation (cf. persona [Administrateur](USER_PERSONAS.md)),
- permettre une régularisation simple (montée en gamme ou ajustement), sans interruption brutale du service pour l'organisation.

## 5. Granularité fonctionnelle

Le système de licences doit permettre d'activer ou de restreindre l'accès à un module fonctionnel de façon indépendante, afin de refléter fidèlement les différences de périmètre entre les offres définies dans [BUSINESS_MODEL.md](BUSINESS_MODEL.md), sans dupliquer le logiciel selon l'offre souscrite.

## 6. Séparation entre organisations

Chaque licence est strictement rattachée à une organisation cliente. Les données, utilisateurs et configurations d'une organisation ne sont jamais visibles ni accessibles par une autre organisation, quel que soit le niveau de licence respectif.

## 7. Principe de transparence

L'administrateur d'une organisation cliente doit pouvoir consulter à tout moment, de façon claire, l'état de sa licence : offre souscrite, périmètre actif, volumes utilisés par rapport aux limites, et date de renouvellement. Ce principe de transparence prévaut sur toute considération commerciale ponctuelle.
