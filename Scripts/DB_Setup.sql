DROP TABLE "Skip";
DROP TABLE "Listen";
DROP TABLE "Tracks";
DROP TABLE "Artists";
DROP TABLE "Sessions";
DROP TABLE "Spotify";

CREATE TABLE "Spotify" (
    spot_id int IDENTITY(1,1) unique not null,
    PRIMARY key (spot_id),

    spotify_id varchar(64) unique,
    authToken Text,
    refreshToken Text,

    authExpires DATETIME,
    SkipThreshold int DEFAULT 3
);

CREATE TABLE "Artists"(
    art_id int IDENTITY(1,1) UNIQUE not null,
    artist_id varchar(32) unique,
    PRIMARY key (art_id, artist_id),

    details text,
    
    true_at datetime DEFAULT GETDATE()
)

CREATE TABLE "Tracks"(
    trk_id int IDENTITY(1,1) UNIQUE not null,
    track_id varchar(32) unique,
    PRIMARY key (trk_id, track_id),

    artist_id varchar(32),
    foreign key (artist_id) REFERENCES Artists(artist_id),

    title text,
    features text,
    
    true_at datetime DEFAULT GETDATE()
);

CREATE TABLE "Listen"(
    listen_id int IDENTITY(1,1) UNIQUE not null,
    PRIMARY key (listen_id),

    track_id varchar(32),
    foreign key (track_id) REFERENCES Tracks(track_id),

    spot_id int not null,
    foreign key (spot_id) REFERENCES Spotify(spot_id),

    listen_at datetime DEFAULT GETDATE()
);

CREATE TABLE "Skip"(
    skip_id int IDENTITY(1,1) UNIQUE not null,
    PRIMARY key (skip_id),

    track_id varchar(32),
    foreign key (track_id) REFERENCES Tracks(track_id),
    
    spot_id int not null,
    foreign key (spot_id) REFERENCES Spotify(spot_id),

    skip_at datetime DEFAULT GETDATE()
);

CREATE TABLE "Sessions"(
    sess_id int IDENTITY(1,1) unique not null,
    PRIMARY key (sess_id),

    spot_id int not null unique,
    foreign key (spot_id) REFERENCES Spotify(spot_id),

    auth_token VARCHAR(128)
);