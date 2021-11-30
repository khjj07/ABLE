package com.example.myapplication;


import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ViewFlipper;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.google.android.material.bottomnavigation.BottomNavigationView;

//public class MainActivity extends AppCompatActivity implements View.OnClickListener {
public class MainActivity extends AppCompatActivity  {
    final String TAG = this.getClass().getSimpleName();
    LinearLayout home_ly;
    BottomNavigationView bottomNavigationView;

    HomeFragment homeFragment;
    MapFragment mapFragment;
    AskFragment askFragment;

//    ImageButton prev;
//    ImageButton next;
//    ViewFlipper flipper;
//
//    ImageView img1;
//    ImageView img2;
//    ImageView img3;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        init();
        SettingListener();

//        Button executeUnity = (Button)findViewById(R.id.executeUnity);
//        executeUnity.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(getApplicationContext(), UnityPlayerActivity.class);
//                startActivity(intent);
//            }
//        });
        homeFragment = new HomeFragment();
        mapFragment = new MapFragment();
        askFragment = new AskFragment();


        //제일 처음 띄워줄 뷰를 세팅해줍니다. commit();까지 해줘야 합니다.
        getSupportFragmentManager().beginTransaction().replace(R.id.home_ly,homeFragment).commit();




        //bottomNavigationView.setSelectedItemId(R.id.home);




//        flipper = (ViewFlipper) findViewById(R.id.flipper);
//        prev = (ImageButton) findViewById(R.id.prev);
//        next = (ImageButton) findViewById(R.id.next);
//
//        prev.setOnClickListener(this);
//        next.setOnClickListener(this);
//
//        img1 = (ImageView) findViewById(R.id.img1);
//        img2 = (ImageView) findViewById(R.id.img2);
//        img3 = (ImageView) findViewById(R.id.img3);


    }

    private void init() {
        home_ly = findViewById(R.id.home_ly);
        bottomNavigationView = findViewById(R.id.bottomNavigationView);

    }

    private void SettingListener() {
        bottomNavigationView.setOnNavigationItemSelectedListener(new TabSelectedListener());
    }

    class TabSelectedListener implements BottomNavigationView.OnNavigationItemSelectedListener {
        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
            switch (menuItem.getItemId()) {
                case R.id.tab_home: {
                    onFragmentChange(0);
                    return true;
                }

                case R.id.tab_map: {
                    onFragmentChange(1);
                    return true;
                }

                case R.id.tab_ask: {
                    onFragmentChange(2);
                    return true;
                }
            }
            return false;

        }

    }

    //프래그먼트와 프래그먼트끼리 직접접근을하지않는다. 프래그먼트와 엑티비티가 접근함
    public void onFragmentChange(int index){
        if(index == 0 ){
            getSupportFragmentManager().beginTransaction().replace(R.id.home_ly,  homeFragment).commit();
        }else if(index == 1){
            getSupportFragmentManager().beginTransaction().replace(R.id.home_ly, mapFragment).commit();
        }else if(index == 2){
            getSupportFragmentManager().beginTransaction().replace(R.id.home_ly, askFragment).commit();
        }
    }

//    //ViewFlipper 클릭 이벤트 핸들러
//    public void onClick(View v) {
//        if(v == prev) {
//            flipper.showPrevious();
//        }
//        else if(v == next) {
//            flipper.showNext();
//        }
//    }
}