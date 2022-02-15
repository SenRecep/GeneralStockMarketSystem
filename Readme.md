# General Stock Market
## _Borsa Simülasyonu_



[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

General Stock Market, basit borsa işlemlerinin simüle edilebildiği bir yazılım projesidir.

- Emirlerin girilmesi.
- Fiyat belirlenmesi.
- Ürün ve para giriş isteği yapılması.

## Özellikler

- Kullanıcı validasyonu.
- Rol sistemi ve yetkilendirme.
- Tamamen asenkron çalışma yapısı.
- SOLID prensiplerine uygun geliştirme.
- Repository ve Unit of Work desenlerinin uygulanması.
- Code First Database tasarımı.
- Web Api(.Net 5) sistemi içerisinde katmanlı mimari kullanımı.
- Swagger Dokümantasyonu.
- Identity Server(.Net Core 3.1) kullanımıyla kapsamlı kullanıcı ve token yönetimi.
- Blazor(.Net 5) ile single page modern reactive uygulama geliştirme.
- Entity Framework Core 5.


## Teknolojiler

General Stock Market performanslı çalışabilmek için bazı açık kaynaklı teknolojileri kullanır.


- [Twitter Bootstrap] - Modern web uygulamaları için arayüz stili
- [jQuery] - Cliend Side Validation işlemleri
- [Blazor Server App] - C# tabanlı Javascript alternatifi 
- [scss] - Kolay düzenlenebilir gelişmiş css yapısı.

## Kullanım
Başlamak için öncelikle sisteme üye olmak gerekiyor. Üye olunduktan sonra kullanıcımız henüz doğrulanmamış olarak sisteme giriş yapıyor.

Doğrulanmamış kullanıcılar sadece market fiyatlarını görüntüleyebiliyor. İşlem yapmak için hesap sayfasından gerekli bilgileri doldurmuş olmak gerekiyor. Kullanıcımız doğrulandığında artık istek, emir ve borsa işlemlerine erişim sağlayabiliyor.

Herhangi bir borsa işlemi gerçekleştirmek için önce sisteme kullanıcının varlıklarını istek olarak girmesi gerekiyor(Örn. İtibari para, ürün veya yeni ürün tipi). İstekleri yöneticiler tarafından kabul edilen kullanıcıların varlıkları cüzdanlarına tanımlanıyor. 

Bu aşamadan sonra alım ve satım işlemleri sorunsuzca gerçekleştirilebiliyor.

Son olarak kullanıcı sistemde olmayan bir varlığı girmek için yeni ürün tipi isteği oluşturarak bu ürünü sisteme yönetici izni dahilinde ekleyebiliyor.

## Lisans

MIT

**Herkes için özgür yazılım.**

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

   [dill]: <https://github.com/joemccann/dillinger>
   [git-repo-url]: <https://github.com/joemccann/dillinger.git>
   [john gruber]: <http://daringfireball.net>
   [df1]: <http://daringfireball.net/projects/markdown/>
   [markdown-it]: <https://github.com/markdown-it/markdown-it>
   [Ace Editor]: <http://ace.ajax.org>
   [node.js]: <http://nodejs.org>
   [Twitter Bootstrap]: <http://twitter.github.com/bootstrap/>
   [jQuery]: <http://jquery.com>
   [@tjholowaychuk]: <http://twitter.com/tjholowaychuk>
   [express]: <http://expressjs.com>
   [AngularJS]: <http://angularjs.org>
   [Gulp]: <http://gulpjs.com>
   [Blazor Server App]:<https://docs.microsoft.com/tr-tr/aspnet/core/blazor/?view=aspnetcore-5.0>
   [scss]:<sass-lang.com>

   [PlDb]: <https://github.com/joemccann/dillinger/tree/master/plugins/dropbox/README.md>
   [PlGh]: <https://github.com/joemccann/dillinger/tree/master/plugins/github/README.md>
   [PlGd]: <https://github.com/joemccann/dillinger/tree/master/plugins/googledrive/README.md>
   [PlOd]: <https://github.com/joemccann/dillinger/tree/master/plugins/onedrive/README.md>
   [PlMe]: <https://github.com/joemccann/dillinger/tree/master/plugins/medium/README.md>
   [PlGa]: <https://github.com/RahulHP/dillinger/blob/master/plugins/googleanalytics/README.md>
