using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class FriendPictureScript : MonoBehaviour
{
    public GameObject friendPfp;

    private Steamworks.SteamId idFriend;
    async void Start()
    {
        if (!SteamClient.IsValid) return;
        
        // GET RANDOM FRIEND PIC
        var steamFriends = SteamFriends.GetFriends();
        int nbFriends = steamFriends.Count();
        Friend friend = steamFriends.ElementAtOrDefault(Random.Range(0, nbFriends));
        Debug.Log(friend.Name);
        idFriend = friend.Id;
        var imageFriend = await SteamFriends.GetMediumAvatarAsync(friend.Id);
        Texture2D friendPfPImage = GetTextureFromImage(imageFriend.Value);
        // ADD IF NO FRIENDS

        // APPLY    
        SpriteRenderer spfriendPfp = friendPfp.GetComponent<SpriteRenderer>();
        spfriendPfp.material.SetTexture("_OverlayTex", friendPfPImage);

        

    }


    public static Texture2D GetTextureFromImage(Steamworks.Data.Image image) // CONVERTISSEUR
    {
        Texture2D texture = new Texture2D((int)image.Width, (int)image.Height);
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var p = image.GetPixel(x, y);
                texture.SetPixel(x, (int)image.Height - y, new Color (p.r/ 255.0f, p.g/ 255.0f, p.b / 255.0f, p.a / 255.0f));
            }
        }
        texture.Apply();
        return texture;
    }
}
