using System;
using Unity.Mathematics;
using UnityEngine;

public class launcher : MonoBehaviour
{
 //snelheid waarmee de lijn groeit
    [SerializeField] private float lineSpeed = 10f;
    //verwijzing naar de linerenderer
    private LineRenderer _line;
    //we houden hiermee bij of de lijn actief is of niet
    private bool _lineActive = false;

    [SerializeField] private GameObject prefab;
    //kracht die de bal krijgt per seconde dat we de knop inhouden
    [SerializeField] private float ballOffset = 1.5f;
    [SerializeField] private float forceBuild = 50f;
    //maximale tijd om bij te houden hoe lang we de knop hebben ingedrukt
    [SerializeField] private float maximumHoldTime = 5f;

    //Deze variabelen zijn niet zichtbaar in de inspector

    //Bijhouden hoe lang we de knop hebben ingedrukt (seconden)
    private float _pressTimer = 0f;
    //Totale kracht waarmee de bal wordt afgevoord
    private float _launchForce = 0f;




    float playerRotation = 0.0f;
    public float turnSpeed = 180.0f;
    public Vector3 defaultRotation = new Vector3(0,0,180);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //we vragen het Line Renderer component op en slaan deze op in een variabele zodat we er later dingen mee kunnen doen
        _line = GetComponent<LineRenderer>();
        //We pakken het eindpunt van de lijn en zetten deze op positie 0,0,0 (zelfde plek als het beginpunt). Hierdoor word de lijn onzichtbaar. Punt 0 is het beginpunt en punt 1 het eindpunt.
        _line.SetPosition(1,Vector3.zero);
        //_line.SetPosition(0,Vector3.one); zou het beginpunt aanpassen. Maar dat is niet nodig nu.
    }

    // Update is called once per frame
    void Update()
    {
        playerRotation += Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(defaultRotation.x, defaultRotation.y, defaultRotation.z+playerRotation);
        HandleShot();
    }

    private void HandleShot() {
        //Check of de linkermuisknop word ingedrukt (alleen het eerste moment van indrukken)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _pressTimer = 0; //reset de timer weer op 0. Verderop gaan we de tijd hierin bijhouden hoe lang we de knop hebben ingehouden
            _lineActive = true;
        }
        //Check of je de linkermuisknop loslaat.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            /*bepaal de kracht die je bal moet krijgen. hoe langer je de knop hebt vastgehouden hoe meer kracht. Met forcebuild kun je deze kracht tweaken in de inspector. Dit is de kracht per seconde.*/
            _launchForce = _pressTimer * forceBuild;

            /*Instantiate maakt van een prefab een gameonject in je scene.
            Er wordt dus een nieuwe bal in je scene aangemaakt.
            Om nog meer met deze bal te kunnen in ons script slaan we hem op in een variabele
            transform.parent verwijst naar de scene zodat de bal in de scene beland en niet in je kannon */
            GameObject ball = Instantiate(prefab, transform.parent);

            /*geef de bal dezelfde rotatie als het kanon zodat we heb de juiste richting op kunnen schieten.*/
            ball.transform.rotation = transform.rotation;

            /*Geef de Rigidbody van de bal een kracht (_launchForce) naar rechts mee op zijn eigen x-as. Doordat de bal goed geroteerd is gaat hij de goede kant op. ForceMode2D.Impulse zorgt dat alle kracht in 1 keer aan de bal gegeven wordt*/
            ball.GetComponent<Rigidbody>().AddForce((-ball.transform.up) * _launchForce, ForceMode.Impulse);

            /*Plaats de bal op dezelfde plek als het kanon zodat deze op die plek in de scene verschijnt*/
            ball.transform.position = new Vector3(transform.position.x,transform.position.y,-3) - (transform.up*ballOffset);

            _lineActive = false;
        _line.SetPosition(1, Vector3.zero);
        }
        /*Om te voorkomen dat we oneindige kracht mee kunnen geven beperken we de tijd die we maximaal bij gaan houden. Deze maximum tijd kunnen we in seconden instellen in de inspector (maximumHoldTime)*/
        if(_pressTimer < maximumHoldTime){
            /*Elk frame tellen we de duur van het frame op bij de verstreken tijd sinds we de knop in hebben gedrukt. Zodra we deze los laten weten we dus hoe lang dit duurde */
            _pressTimer += Time.deltaTime;
        }
        if (_lineActive) {
        _line.SetPosition(1, Vector3.down * _pressTimer * lineSpeed);
    }
    }
}
