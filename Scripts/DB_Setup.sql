DROP TABLE "Skip";
DROP TABLE "Listen";
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

CREATE TABLE "Listen"(
    listen_id int IDENTITY(1,1) UNIQUE not null,
    PRIMARY key (listen_id),

    track_id varchar(32),
    spot_id int not null,
    foreign key (spot_id) REFERENCES Spotify(spot_id),

    features text,

    listen_at datetime DEFAULT GETDATE()
);

CREATE TABLE "Skip"(
    skip_id int IDENTITY(1,1) UNIQUE not null,
    PRIMARY key (skip_id),

    track_id varchar(32),
    spot_id int not null,
    foreign key (spot_id) REFERENCES Spotify(spot_id),

    skip_at datetime DEFAULT GETDATE()
);

CREATE TABLE "Sessions"(
    sess_id int IDENTITY(1,1) unique not null,
    PRIMARY key (sess_id),

    spot_id int not null,
    foreign key (spot_id) REFERENCES Spotify(spot_id),

    auth_token VARCHAR(128)
);