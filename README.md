# QuickGameFinder
Projet réalisé sous Electron.JS

// DISCLAIMER //

Ce projet est une version améliorée de celui créé à l'origine en C#. Je le développe actuellement dans mon temps libre.

// /DISCLAIMER //

QuickGameFinder est une plateforme permettant aux joueurs n'ayant pas de cohéquipier avec lequel jouer de trouver rapidement une équipe qui correspond à ses attentes pour lancer une partie sans avoir la mauvaise surprise de tomber avec une équipe bancale du au hasard du matchmaking des jeux en ligne.

Ce projet est réalisé en JavaScript pour l'aspect back-end (NODEJS) et en HTML, CSS et également JavaScript pour l'aspect front-end.

Comment ça fonctionne :

Ce projet repose sur une structure réseau utilisant le protocole TCP. Ce protocole est souvent utilisé dans les jeux comme World Of Warcraft pour transmettre des packets réseau du client au serveur et inversement en temps réel sans a avoir a passer par des requêtes POST et GET a une API.

Dans notre cas le serveur aura le contrôle des connexions, inscriptions, la gestion des salons, les messages etc...

Ce projet reprend le concept de base de celui que j'ai réalisé en C# il y'a un an mais en améliorant considérablement l'aspect graphique et technique de celui-ci.


// NOTE //

Les packets réseau échangés entre le serveur et le client n'ont volontairement pas été encrypté car ce projet est purement personnel, développé par passion et ne sera pas déployé publiquement, les données qui y sont stockées n'ont donc pas besoin d'être sécurisées.

// Work In Progress //

![alt text](https://i.ibb.co/pPhF12w/Screenshot-1.png)
![alt text](https://i.ibb.co/vsFG24L/Screenshot-2.png)
![alt text](https://i.ibb.co/82Y4ZdK/Screenshot-3.png)
