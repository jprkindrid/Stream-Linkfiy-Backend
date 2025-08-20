using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Mappers
{
    public static class AlbumMapper
    {
        public static AlbumReturnDto ToAlbumReturnDto(this AlbumModel album)
        {
            return new AlbumReturnDto
            {
                ArtistNames = album.AritstNames,
                AlbumName = album.AlbumName,
                Spotify = album.SpotifyUrl,
                AppleMusic = album.AppleMusicUrl,
                Tidal = album.TidalUrl,
                Deezer = album.DeezerUrl
            };
        }
    }
}
