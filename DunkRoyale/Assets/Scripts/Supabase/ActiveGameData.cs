using System;

//Clase para obtener las cosas de vista marcador
[Serializable]
public class ActiveGameData
{
	public string game_id;
	public string lakers_name;
	public string lakers_logo;
	public int lakers_score;
	public string opposing_team_name;
	public string opposing_team_logo;
	public int opposing_score;
	public bool home;
	public string start_date;
	public string venue;
	public int attended;
}
