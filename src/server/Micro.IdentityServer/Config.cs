// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Micro.IdentityServer
{
    public static class Config
    {
        //audience
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"/*, "discount_readpermission", "discount_writepermission"*/}},
            new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
            new ApiResource("resource_payment"){Scopes={"payment_fullpermission"}},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        //claims
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name="roles",
                DisplayName="Roles",
                Description = "User Roles.",
                UserClaims = new[] { "role" }
            }
        };

        //permissions
        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalog_fullpermission","Catalog api için full erişim."),
            new ApiScope("photo_stock_fullpermission","Photo Stock api için full erişim."),
            new ApiScope("basket_fullpermission","Basket api için full erişim."),
            new ApiScope("discount_fullpermission","discount api için full erişim."),
            //new ApiScope("discount_readpermission","Discount api için okuma erişimi."),
            //new ApiScope("discount_writepermission","Discount api için yazma erişimi."),
            new ApiScope("order_fullpermission","order api için full erişim."),
            new ApiScope("payment_fullpermission","payment api için full erişim."),
            new ApiScope("gateway_fullpermission","Gateway API için full erişim"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

        //clients
        public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client  //standart user
            {
                ClientName="standart",
                ClientId="standart-id",
                ClientSecrets= {new Secret("standart-secret".Sha256())},
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                AllowedScopes={
                    "catalog_fullpermission",
                    "photo_stock_fullpermission",
                    "gateway_fullpermission",
                    IdentityServerConstants.LocalApi.ScopeName
                }
            },
            new Client //custom user
            {
                ClientName="custom",
                ClientId="custom-id",
                AllowOfflineAccess = true,
                ClientSecrets= {new Secret("custom-secret".Sha256())},
                AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,                
                AllowedScopes={ //Erişim izinleri
                    "basket_fullpermission",
                    "discount_fullpermission",
                    //"discount_readpermission",
                    //"discount_writepermission",
                    "order_fullpermission",
                    "payment_fullpermission",
                    "gateway_fullpermission",
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,    //refresh token
                    IdentityServerConstants.LocalApi.ScopeName,
                    "roles"
                },
                AccessTokenLifetime = 1*60*60,  //1 saat
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds, // refresh token süresi
                RefreshTokenUsage = TokenUsage.ReUse    //yeni token alınca refresh token yenilenecek.
            }
        };
    }
}