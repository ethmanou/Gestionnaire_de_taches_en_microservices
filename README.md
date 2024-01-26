Bienvenue dans mon projet de microservices ! Il consiste à créer une application reposant sur trois microservices essentiels : une Gateway, qui relie le frontend aux autres microservices (UserService et TaskService). Le UserService gère les utilisateurs, tandis que le TaskService s'occupe des tâches associées à ces utilisateurs.

Le projet est dirigé par moi-même, Mohamedene Ethmanou, et je suis ravi de le partager avec vous ! N'hésitez pas à me contacter pour des questions ou des retours à Mohamedene.ETHMANOU@etu.uca.fr.

Pour faciliter les tests, j'ai déjà créé deux utilisateurs avec admin :

- **Utilisateur 1 :**
  - Nom d'utilisateur : ethmanou
  - Mot de passe : ethmanou

- **Utilisateur 2 :**
  - Nom d'utilisateur : mohamed
  - Mot de passe : mohamed

- **Administarteur :**
  - Nom d'utilisateur : admin
  - Mot de passe : admin

Bien sûr, vous avez la possibilité de créer de nouveaux utilisateurs avec des tâches pour tester davantage l'application.



J'ai ajouté les bonus suivants :

Ajout d'un champ de rôle aux utilisateurs (basique, admin) : J'ai créé deux nouvelles pages, ALLUsers et ALLTasks, qui permettent de consulter tous les utilisateurs ou toutes les tâches. De plus, j'ai ajouté la possibilité de supprimer ou mettre à jour les tâches, ainsi que de modifier le rôle de l'utilisateur ou de le supprimer.

Gestion des erreurs : J'ai pris en charge les erreurs du côté de la passerelle, afin qu'elle ne renvoie pas d'informations sensibles au front-end. J'ai effectué la même démarche du côté du front-end, en utilisant des blocs try-catch. Pour certains cas, j'ai créé une classe d'exception générale, et pour d'autres, une classe ExceptionRequest, car une exception générique suffit dans ces situations.

Persistance des données du second micro-service : J'ai mis en place une base de données avec des migrations pour assurer la persistance des données du second micro-service.

Ajout d'un code Konami : J'ai mis en place un service dédié, utilisant des suites de flèches pour implémenter le code Konami.

Intégration de fonctionnalités supplémentaires : Ajout d'une fonctionnalité DeadLine et DoneDate pour les tâches, permettant de vérifier si l'utilisateur a terminé ses tâches à temps. Les informations sont affichées en conséquence.

